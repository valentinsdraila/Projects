﻿using PaymentService.Model.Messages;
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
            var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
            _connection = await RetryAsync(() => factory.CreateConnectionAsync());
            _channel = await _connection.CreateChannelAsync();
            await _channel.QueueDeclareAsync(queue: "payment_update", durable: true, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: "product_user_interaction", durable: true, exclusive: false, autoDelete: false, arguments: null);
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

        public async Task SendStockRollback(StockRollbackMessage stockRollbackMessage)
        {
            var json = JsonSerializer.Serialize(stockRollbackMessage);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(exchange: string.Empty,
                                      routingKey: "stock_rollback",
                                      body: body);
            Console.WriteLine($"[PaymentService] Sent stock rollback for the Order {stockRollbackMessage.OrderId}");
        }

        public async Task SendUserInteraction(UserInteractionMessage userInteractionMessage)
        {
            var json = JsonSerializer.Serialize(userInteractionMessage);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(exchange: string.Empty,
                                      routingKey: "product_user_interaction",
                                      body: body);
            Console.WriteLine($"[PaymentService] Sent user interaction message. User:{userInteractionMessage.UserId}, Product:{userInteractionMessage.ProductId}, Action: Paid");
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
