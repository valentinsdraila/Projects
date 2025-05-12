using PaymentService.Model;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PaymentService.Services
{
    /// <summary>
    /// Contains methods for publishing messages through the RabbitMQ amqp.
    /// </summary>
    public class RabbitMQPublisher
    {
        /// <summary>
        /// Sends the payment status update message to OrderService.
        /// </summary>
        /// <param name="paymentStatusUpdateMessage">The order stock update message.</param>
        public async Task SendPaymentStatus(PaymentStatusUpdateMessage paymentStatusUpdateMessage)
        {
            var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "payment_update", durable: true, exclusive: false, autoDelete: false, arguments: null);
            var json = JsonSerializer.Serialize(paymentStatusUpdateMessage);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(exchange: string.Empty,
                                      routingKey: "payment_update",
                                      body: body);
            Console.WriteLine($"[PaymentService] Sent payment status update for the Order {paymentStatusUpdateMessage.Id}");
        }
    }
}
