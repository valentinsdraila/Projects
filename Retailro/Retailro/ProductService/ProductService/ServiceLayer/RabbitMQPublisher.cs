using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using ProductService.Model;

namespace ProductService.ServiceLayer
{
    /// <summary>
    /// Contains methods for publishing messages through the RabbitMQ amqp.
    /// </summary>
    public class RabbitMQPublisher : IAsyncDisposable
    {
        private IChannel _channel;
        private IConnection _connection;

        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
            _connection = await RetryAsync(() => factory.CreateConnectionAsync());
            _channel = await _connection.CreateChannelAsync();
            await _channel.QueueDeclareAsync(queue: "product_user_interaction", durable: true, exclusive: false, autoDelete: false, arguments: null);

        }

        /// <summary>
        /// Sends the user interaction message to RecommendationService.
        /// </summary>
        /// <param name="userInteractionMessage">The order stock update message.</param>

        public async Task SendUserInteraction(UserInteractionMessage userInteractionMessage)
        {
            var json = JsonSerializer.Serialize(userInteractionMessage);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(exchange: string.Empty,
                                      routingKey: "product_user_interaction",
                                      body: body);
            Console.WriteLine($"[ProductService] Sent user interaction message. User:{userInteractionMessage.UserId}, Product:{userInteractionMessage.ProductId}, Action:{userInteractionMessage.Action}");

        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
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
    }
}
