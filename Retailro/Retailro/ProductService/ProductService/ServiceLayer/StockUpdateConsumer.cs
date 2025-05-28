using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using ProductService.DataLayer;
using ProductService.Model;
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
    /// <summary>
    /// Hosted service used for handling the communication with other services through the RabbitMQ amqp.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
    public class StockUpdateConsumer : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private IConnection connection;
        private IChannel channel;

        public StockUpdateConsumer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// Declares callbacks used to treat the events sent by other services.
        /// Also sends messages to other services asynchronously after it finishes its job.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {

            var factory = new ConnectionFactory { HostName = "rabbitmq", Port = 5672 };
            connection = await RetryAsync(() => factory.CreateConnectionAsync());
            channel = await RetryAsync(() => connection.CreateChannelAsync());

            await channel.ExchangeDeclareAsync("stock_confirmation_exchange", ExchangeType.Fanout);

            await channel.QueueDeclareAsync(queue: "stock_update", durable: true, exclusive: false, autoDelete: false, arguments: null);

            await channel.QueueDeclareAsync(queue: "stock_rollback", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var stockUpdateMessage = JsonSerializer.Deserialize<StockUpdateMessage>(message);

                    Console.WriteLine($"[ProductService] Received stock update for Order");
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                        bool allSuccessful = true;

                        using var transaction = await dbContext.Database.BeginTransactionAsync();
                        try
                        {
                            foreach (var stockUpdate in stockUpdateMessage.StockUpdates)
                            {
                                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == stockUpdate.ProductId);

                                if (product == null || product.Quantity < stockUpdate.Quantity)
                                {
                                    allSuccessful = false;
                                    break;
                                }

                                if (product.UnitPrice != stockUpdate.UnitPrice)
                                    throw new PriceNotMatchingException($"The price is not the same as the original for product {stockUpdate.ProductId}");

                                product.Quantity -= stockUpdate.Quantity;

                            }
                            if (allSuccessful)
                            {
                                //The stocks for all products were successfully updated.
                                await dbContext.SaveChangesAsync();
                                await transaction.CommitAsync();
                                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                                var stockUpdateResponse = new StockConfirmation(stockUpdateMessage.OrderId, true);
                                var json = JsonSerializer.Serialize(stockUpdateResponse);
                                var bodyToSend = Encoding.UTF8.GetBytes(json);

                                await channel.BasicPublishAsync(exchange: "stock_confirmation_exchange",
                                routingKey: string.Empty,
                                body: bodyToSend);

                            }
                            else
                            {
                                //The stock was not enough for some of the products, or some products were not found.
                                //Order should get cancelled.
                                var stockUpdateResponse = new StockConfirmation(stockUpdateMessage.OrderId, false);
                                var json = JsonSerializer.Serialize(stockUpdateResponse);
                                var bodyToSend = Encoding.UTF8.GetBytes(json);
                                await channel.BasicPublishAsync(exchange: "stock_confirmation_exchange",
                                routingKey: string.Empty,
                                body: bodyToSend);

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

            var stockRollbackConsumer = new AsyncEventingBasicConsumer(channel);
            stockRollbackConsumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockRollbackMessage = JsonSerializer.Deserialize<StockRollbackMessage>(message);

                Console.WriteLine($"[ProductService] Received stock rollback for Order {stockRollbackMessage.OrderId}");
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
                    var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                    bool allSuccessful = true;

                    using var transaction = await dbContext.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (var stockUpdate in stockRollbackMessage.StockUpdates)
                        {
                            var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == stockUpdate.ProductId);

                            if (product == null)
                            {
                                allSuccessful = false;
                                break;
                            }

                            product.Quantity += stockUpdate.Quantity;

                        }
                        if (allSuccessful)
                        {
                            //The stocks for all products were successfully updated.
                            await dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        else
                        {
                            //Some of the ProductIds were invalid, the transaction fails.
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
                }
            };

            await channel.BasicConsumeAsync(queue: "stock_rollback", autoAck: false, consumer: stockRollbackConsumer);


            Console.WriteLine("[ProductService] Waiting for stock update messages...");
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// Releases the resources of the channel and connection instances.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
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

        /// <summary>
        /// Records used for parsing and sending messages.
        /// </summary>
        private record StockUpdate(Guid ProductId, int Quantity, decimal UnitPrice);
        private record StockUpdateMessage(Guid OrderId, List<StockUpdate> StockUpdates);
        private record StockConfirmation(Guid OrderId, bool Success);
        private record StockRollbackMessage(Guid OrderId, List<StockUpdate> StockUpdates);

    }

}
