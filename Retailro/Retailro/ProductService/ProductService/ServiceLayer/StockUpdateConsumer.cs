using Microsoft.AspNetCore.Connections;
using ProductService.DataLayer;
using ProductService.Model.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace ProductService.ServiceLayer
{
    public class StockUpdateConsumer : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private IConnection connection;
        private IChannel channel;

        public StockUpdateConsumer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost", Port=5672 };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "stock_update", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockUpdateMessages = JsonSerializer.Deserialize<List<StockUpdateMessage>>(message);

                Console.WriteLine($"[ProductService] Received stock update for Order");
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                    var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                    bool allSuccessful = true;

                    using var transaction = await dbContext.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (var stockUpdate in stockUpdateMessages)
                        {

                            var success = await productRepository.ReduceStock(stockUpdate.ProductId, stockUpdate.Quantity, stockUpdate.UnitPrice);
                            if(!success)
                            {
                                allSuccessful = false;
                                break;
                            }    

                        }
                        if(allSuccessful)
                        {
                            //The stocks for all products were successfully updated.
                            await dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                            await channel.BasicAckAsync(deliveryTag:ea.DeliveryTag, multiple:false);
                        }
                        else
                        {
                            //The stock was not enough for some of the products, or some products were not found.
                            //Order should get cancelled.
                            await transaction.RollbackAsync();
                            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                    }
                    catch (DBConcurrencyException)
                    {
                        //Concurrency issue when saving the changes, the transaction is retried.
                        await transaction.RollbackAsync();
                        await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                    catch (PriceNotMatchingException ex)
                    {
                        //A message indicating the order fails will be sent
                        await transaction.RollbackAsync();
                        await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                        Console.WriteLine(ex.Message);
                    }
                }
            };

            await channel.BasicConsumeAsync(queue: "stock_update", autoAck: false, consumer: consumer);

            Console.WriteLine("[ProductService] Waiting for stock update messages...");
        }

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

        private record StockUpdateMessage(Guid ProductId, int Quantity, decimal UnitPrice);
    }

}
