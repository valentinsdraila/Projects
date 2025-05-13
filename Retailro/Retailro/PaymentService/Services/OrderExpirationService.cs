using PaymentService.Model;
using PaymentService.Model.Messages;

namespace PaymentService.Services
{
    public class OrderExpirationService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<OrderExpirationService> _logger;

        public OrderExpirationService(IServiceProvider services, ILogger<OrderExpirationService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();
                var redis = scope.ServiceProvider.GetRequiredService<RedisService>();
                var publisher = scope.ServiceProvider.GetRequiredService<RabbitMQPublisher>();

                var expiredOrders = await redis.GetExpiredOrdersAsync();
                foreach (var orderId in expiredOrders)
                {
                    var order = await redis.GetNoTTLOrderAsync(orderId);
                    if (order == null) continue;

                    if (order.Status == OrderStatus.Processing || order.Status == OrderStatus.Valid)
                    {
                        _logger.LogInformation("Cancelling expired order {OrderId}", orderId);
                        await publisher.SendPaymentStatus(new PaymentStatusUpdateMessage { Id = orderId, Status = OrderStatus.Cancelled });
                        await publisher.SendStockRollback(new StockRollbackMessage { OrderId = orderId, StockUpdates = order.StockUpdates });
                    }

                    await redis.RemoveOrderAsync(orderId);
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

}
