using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using PaymentService.Model;
using System.Linq.Expressions;
using PaymentService.Model.Messages;

namespace PaymentService.Services
{
    /// <summary>
    /// Hosted service that contains methods for consuming events over the RabbitMQ amqp.
    /// </summary>
    public class EventsConsumer : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private IConnection connection;
        private IChannel channel;

        public EventsConsumer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        /// <summary>
        /// Starts the hosted service asynchronously.
        /// Receives events from other services in the system and uses callbacks to treat them.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync("stock_confirmation_exchange", ExchangeType.Fanout);

            await channel.QueueDeclareAsync(queue: "order_created", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueDeclareAsync(queue: "payment_stock_confirmation", durable: true, exclusive: false, autoDelete: false, arguments: null);

            await channel.QueueBindAsync("payment_stock_confirmation", "stock_confirmation_exchange", routingKey: string.Empty);

            var stockConfirmationConsumer = new AsyncEventingBasicConsumer(channel);
            stockConfirmationConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockConfirmationMessage = JsonSerializer.Deserialize<StockConfirmationMessage>(message);

                Console.WriteLine($"Received stock confirmation from ProductService for order {stockConfirmationMessage.OrderId}");
                using (var scope = serviceProvider.CreateScope())
                {
                    try
                    {
                        var redis = scope.ServiceProvider.GetRequiredService<RedisService>();
                        var order = await redis.GetOrderAsync(stockConfirmationMessage.OrderId);
                        if (order != null)
                        {
                            order.Status = stockConfirmationMessage.Success ? Model.OrderStatus.Valid : Model.OrderStatus.Cancelled;
                            await redis.SetOrderStatusAsync(orderId: stockConfirmationMessage.OrderId, order.Status, order.Total, order.StockUpdates, TimeSpan.FromMinutes(15));
                            await channel.BasicAckAsync(ea.DeliveryTag, false);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Redis error when retrieving the order, retrying...");
                        await Task.Delay(2000);
                        await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                }
            };

            var orderCreatedConsumer = new AsyncEventingBasicConsumer(channel);
            orderCreatedConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var orderCreatedMessage = JsonSerializer.Deserialize<OrderCreatedMessage>(message);

                Console.WriteLine($"Received order creation from OrderService for order {orderCreatedMessage.OrderId}");
                using (var scope = serviceProvider.CreateScope())
                {
                    try
                    {
                        var redis = scope.ServiceProvider.GetRequiredService<RedisService>();
                        await redis.SetOrderStatusAsync(orderId: orderCreatedMessage.OrderId, orderCreatedMessage.Status, orderCreatedMessage.Total, orderCreatedMessage.StockUpdates);
                        await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Redis error when adding the order, retrying...");
                        await Task.Delay(2000);
                        await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                }
            };

            await channel.BasicConsumeAsync(queue: "payment_stock_confirmation", autoAck: false, consumer: stockConfirmationConsumer);
            await channel.BasicConsumeAsync(queue: "order_created", autoAck: false, consumer: orderCreatedConsumer);
            Console.WriteLine("[PaymentService] Waiting for messages...");
        }
        /// <summary>
        /// Stops the hosted service.
        /// Releases the resources of the channel and connection instances.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (channel != null)
            {
                await channel.CloseAsync();
            }

            if (connection != null)
            {
                await connection.CloseAsync();
            }
        }
        public void Dispose()
        {
            if (channel != null)
            {
                channel.Dispose();
            }
            if (connection != null)
            {
                connection.Dispose();
            }
        }

        private record StockConfirmationMessage(Guid OrderId, bool Success);
        private record OrderCreatedMessage(Guid OrderId, decimal Total, OrderStatus Status, List<StockUpdate> StockUpdates);
    }
}
