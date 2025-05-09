using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using OrderService.DataLayer;

namespace OrderService.ServiceLayer
{
    /// <summary>
    /// Hosted service used for handling Stock Confirmation messages
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
    public class StockConfirmationConsumer : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private IConnection connection;
        private IChannel channel;

        public StockConfirmationConsumer(IServiceProvider serviceProvider)
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
            var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "order_stock_confirmation", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync("order_stock_confirmation", "stock_confirmation_exchange", routingKey: string.Empty);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockConfirmationMessage = JsonSerializer.Deserialize<StockConfirmationMessage>(message);

                Console.WriteLine($"Received stock confirmation from ProductService for order{stockConfirmationMessage.OrderId}");
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
            await channel.BasicConsumeAsync(queue: "order_stock_confirmation", autoAck: true, consumer: consumer);
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
                channel.Dispose();
            }

            if (connection != null)
            {
                await connection.CloseAsync();
                connection.Dispose();
            }
        }
        private record StockConfirmationMessage(Guid OrderId, bool Success);
    }
}
