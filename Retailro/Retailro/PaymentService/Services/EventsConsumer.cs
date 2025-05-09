using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using PaymentService.Model;

namespace PaymentService.Services
{
    /// <summary>
    /// Hosted service that contains methods for consuming events over the RabbitMQ amqp.
    /// </summary>
    public class EventsConsumer : IHostedService
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
            var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

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
                    var redis = scope.ServiceProvider.GetRequiredService<RedisService>();
                    var order = await redis.GetOrderAsync(stockConfirmationMessage.OrderId);
                    if (order != null)
                    {
                        order.Status = stockConfirmationMessage.Success ? Model.OrderStatus.Valid : Model.OrderStatus.Cancelled;
                        await redis.SetOrderStatusAsync(orderId: stockConfirmationMessage.OrderId, order.Status, order.Total);
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
                    var redis = scope.ServiceProvider.GetRequiredService<RedisService>();
                    await redis.SetOrderStatusAsync(orderId: orderCreatedMessage.OrderId, orderCreatedMessage.Status, orderCreatedMessage.Total);
                }
            };

            await channel.BasicConsumeAsync(queue: "payment_stock_confirmation", autoAck: true, consumer: stockConfirmationConsumer);
            await channel.BasicConsumeAsync(queue: "order_created", autoAck: true, consumer: orderCreatedConsumer);
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
                channel.Dispose();
            }

            if (connection != null)
            {
                await connection.CloseAsync();
                connection.Dispose();
            }
        }
        private record StockConfirmationMessage(Guid OrderId, bool Success);
        private record OrderCreatedMessage(Guid OrderId, decimal Total, OrderStatus Status);
    }
}
