using OrderService.Model;
using OrderService.ServiceLayer;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

/// <summary>
/// Contains methods for publishing messages through the RabbitMQ amqp.
/// </summary>
public class RabbitMQPublisher
{
    /// <summary>
    /// Sends the stock update message to ProductService.
    /// </summary>
    /// <param name="orderStockUpdateMessage">The order stock update message.</param>
    public async Task SendStockUpdate(OrderStockUpdateMessage orderStockUpdateMessage)
    {
        var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "stock_update", durable: true, exclusive: false, autoDelete: false, arguments: null);
        var json = JsonSerializer.Serialize(orderStockUpdateMessage);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange: string.Empty,
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
        var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "order_created", durable: true, exclusive: false, autoDelete: false, arguments: null);
        var json = JsonSerializer.Serialize(orderStockUpdateMessage);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange: string.Empty,
                                  routingKey: "order_created",
                                  body: body);
        Console.WriteLine($"[OrderService] Sent Order Created message to the payment service");
    }
}
