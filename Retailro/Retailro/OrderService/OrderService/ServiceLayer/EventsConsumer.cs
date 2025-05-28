using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using OrderService.DataLayer;
using OrderService.Model;

namespace OrderService.ServiceLayer
{
    /// <summary>
    /// Hosted service used for handling Stock Confirmation messages
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
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
        /// Triggered when the application host is ready to start the service.
        /// Handles Stock Confirmation messages coming from the ProductService to update the status of the order.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
            connection = await RetryAsync(() => factory.CreateConnectionAsync());
            channel = await RetryAsync(() => connection.CreateChannelAsync());

            await channel.ExchangeDeclareAsync("stock_confirmation_exchange", ExchangeType.Fanout);

            await channel.QueueDeclareAsync(queue: "order_stock_confirmation", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync("order_stock_confirmation", "stock_confirmation_exchange", routingKey: string.Empty);

            var stockConfirmationConsumer = new AsyncEventingBasicConsumer(channel);
            stockConfirmationConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockConfirmationMessage = JsonSerializer.Deserialize<StockConfirmationMessage>(message);

                Console.WriteLine($"Received stock confirmation from ProductService for order {stockConfirmationMessage.OrderId}");
                using (var scope = serviceProvider.CreateScope())
                {
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var order = await orderRepository.GetById(stockConfirmationMessage.OrderId);
                    if (order != null)
                    {
                        order.Status = stockConfirmationMessage.Success ? Model.OrderStatus.Valid : Model.OrderStatus.Cancelled;
                        await orderRepository.Update(order);
                    }
                }
            };
            await channel.BasicConsumeAsync(queue: "order_stock_confirmation", autoAck: true, consumer: stockConfirmationConsumer);


            var paymentUpdateConsumer = new AsyncEventingBasicConsumer(channel);
            paymentUpdateConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var paymentUpdateMessage = JsonSerializer.Deserialize<PaymentUpdateMessage>(message);

                Console.WriteLine($"Received payment update message from PaymentService for order {paymentUpdateMessage.Id}");
                using (var scope = serviceProvider.CreateScope())
                {
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var order = await orderRepository.GetById(paymentUpdateMessage.Id);
                    if (order != null)
                    {
                        order.Status = paymentUpdateMessage.Status;
                        await orderRepository.Update(order);
                    }
                }
            };
            await channel.BasicConsumeAsync(queue: "payment_update", autoAck: true, consumer: paymentUpdateConsumer);

            Console.WriteLine("[OrderService] Waiting for messages...");
        }
        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// Releases the resources for the channel and connection instances.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
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
        private async Task<T> RetryAsync<T>(Func<Task<T>> operation, int maxAttempts = 5)
        {
            var delay = TimeSpan.FromSeconds(10);
            var random = new Random();

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (attempt < maxAttempts)
                {
                    int jitter = random.Next(0, 1000);
                    Console.WriteLine($"Retry {attempt}/{maxAttempts} failed: {ex.Message}. Retrying in {delay.TotalSeconds + jitter / 1000.0}s...");
                    await Task.Delay(delay + TimeSpan.FromMilliseconds(jitter));
                    delay *= 2;
                }
            }

            return await operation();
        }

        private record StockConfirmationMessage(Guid OrderId, bool Success);
        private record PaymentUpdateMessage(Guid Id, OrderStatus Status);
    }
}
