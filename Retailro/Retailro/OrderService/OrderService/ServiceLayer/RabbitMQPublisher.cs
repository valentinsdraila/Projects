using Microsoft.EntityFrameworkCore.Metadata;
using OrderService.Model;
using OrderService.ServiceLayer;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

/// <summary>
/// Contains methods for publishing messages through the RabbitMQ amqp.
/// </summary>
public class RabbitMQPublisher : IAsyncDisposable
{

    private IConnection _connection;
    private IChannel _channel;

    public async Task InitializeAsync()
    {
        var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
        _connection = await RetryAsync(() => factory.CreateConnectionAsync());
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: "stock_update", durable: true, exclusive: false, autoDelete: false, arguments: null);
        await _channel.QueueDeclareAsync(queue: "order_created", durable: true, exclusive: false, autoDelete: false, arguments: null);
    }

    /// <summary>
    /// Sends the stock update message to ProductService.
    /// </summary>
    /// <param name="orderStockUpdateMessage">The order stock update message.</param>
    public async Task SendStockUpdate(OrderStockUpdateMessage orderStockUpdateMessage)
    {
        var json = JsonSerializer.Serialize(orderStockUpdateMessage);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(exchange: string.Empty,
                                  routingKey: "stock_update",
                                  body: body);
        Console.WriteLine($"[OrderService] Sent stock update for the Order");
    }

    /// <summary>
    /// Sends the order created message to PaymentService.
    /// </summary>
    /// <param name="orderStockUpdateMessage">The order stock update message.</param>
    public async Task SendOrderCreated(OrderCreatedMessage orderStockUpdateMessage)
    {
        var json = JsonSerializer.Serialize(orderStockUpdateMessage);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(exchange: string.Empty,
                                  routingKey: "order_created",
                                  body: body);
        Console.WriteLine($"[OrderService] Sent Order Created message to the payment service");
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
