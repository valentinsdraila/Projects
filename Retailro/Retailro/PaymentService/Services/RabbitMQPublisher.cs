using PaymentService.Model.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace PaymentService.Services
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
            var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.QueueDeclareAsync(queue: "payment_update", durable: true, exclusive: false, autoDelete: false, arguments: null);

        }

        /// <summary>
        /// Sends the payment status update message to OrderService.
        /// </summary>
        /// <param name="paymentStatusUpdateMessage">The order stock update message.</param>
        
        public async Task SendPaymentStatus(PaymentStatusUpdateMessage paymentStatusUpdateMessage)
        {
            var json = JsonSerializer.Serialize(paymentStatusUpdateMessage);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(exchange: string.Empty,
                                      routingKey: "payment_update",
                                      body: body);
            Console.WriteLine($"[PaymentService] Sent payment status update for the Order {paymentStatusUpdateMessage.Id}");
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
    }
}
