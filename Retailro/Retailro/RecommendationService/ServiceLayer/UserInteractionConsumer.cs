using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using RecommendationService.Model;
using RecommendationService.DataLayer;

namespace RecommendationService.ServiceLayer
{
    /// <summary>
    /// Hosted service that contains methods for consuming events over the RabbitMQ amqp.
    /// </summary>
    public class UserInteractionConsumer : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private IConnection connection;
        private IChannel channel;

        public UserInteractionConsumer(IServiceProvider serviceProvider)
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
            connection = await RetryAsync(() => factory.CreateConnectionAsync());
            channel = await RetryAsync(() => connection.CreateChannelAsync());

            await channel.QueueDeclareAsync(queue: "product_user_interaction", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var userProductInteractionConsumer = new AsyncEventingBasicConsumer(channel);
            userProductInteractionConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var userProductInteractionMessage = JsonSerializer.Deserialize<UserProductInteractionMessage>(message);

                Console.WriteLine($"Received user interaction message: User:{userProductInteractionMessage.UserId}, Product:{userProductInteractionMessage.ProductId}, Action:{userProductInteractionMessage.Action}");
                using (var scope = serviceProvider.CreateScope())
                {
                    try
                    {
                        var userInteractionRepository = scope.ServiceProvider.GetRequiredService<IUserInteractionRepository>();
                        await userInteractionRepository.Add(new UserProductInteraction { UserId = userProductInteractionMessage.UserId, ProductId = userProductInteractionMessage.ProductId, Action = userProductInteractionMessage.Action, Timestamp = userProductInteractionMessage.Timestamp });
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
            await channel.BasicConsumeAsync(queue: "product_user_interaction", autoAck: false, consumer: userProductInteractionConsumer);
            Console.WriteLine("[RecommendationService] Waiting for messages...");
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
        public record UserProductInteractionMessage(Guid UserId, Guid ProductId, InteractionType Action, DateTime Timestamp);
    }
}
