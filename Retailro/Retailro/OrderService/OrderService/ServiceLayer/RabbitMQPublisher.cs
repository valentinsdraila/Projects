using OrderService.Model;
using OrderService.ServiceLayer;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

public class RabbitMQPublisher
{
    public async Task SendStockUpdate(List<OrderStockUpdateMessage> orderStockUpdateMessages)
    {
        var factory = new ConnectionFactory { HostName = "localhost", Port = 5672 };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "stock_update", durable: true, exclusive: false, autoDelete:false, arguments:null);
        var json = JsonSerializer.Serialize(orderStockUpdateMessages);
        var body = Encoding.UTF8.GetBytes(json);

        await Task.Run(() => {
            channel.BasicPublishAsync(exchange: string.Empty,
                                  routingKey: "stock_update",
                                  body: body);
        });
        Console.WriteLine($"[OrderService] Sent stock update for the Order");
    }
}
