using PaymentService.Model;
using PaymentService.Model.Messages;
using StackExchange.Redis;
using System.Text.Json;

namespace PaymentService.Services
{

    /// <summary>
    /// Utility class for interaction with a redis database
    /// </summary>
    public class RedisService
    {
        private readonly IDatabase _database;
        private const string ExpiringOrdersKey = "expiring_orders";

        public RedisService(IConfiguration configuration)
        {
            var connection = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]);
            _database = connection.GetDatabase();
        }
        /// <summary>
        /// Sets the order status asynchronous in the redis database.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="Status">The status.</param>
        /// <param name="Total">The total.</param>
        /// <param name="ttl">The TTL.</param>
        public async Task SetOrderStatusAsync(Guid orderId, OrderStatus Status, decimal Total, List<StockUpdate> StockUpdates, TimeSpan? ttl = null)
        {
            var orderStatus = new
            {
                Status,
                Total,
                StockUpdates
            };

            var json = JsonSerializer.Serialize(orderStatus);
            await _database.StringSetAsync($"order:{orderId}", json, ttl ?? TimeSpan.FromMinutes(15));
            await _database.StringSetAsync($"order_data:{orderId}", json);

            var expiration = DateTimeOffset.UtcNow.Add(ttl ?? TimeSpan.FromMinutes(15)).ToUnixTimeSeconds();
            await _database.SortedSetAddAsync(ExpiringOrdersKey, orderId.ToString(), expiration);
        }
        /// <summary>
        /// Updates the order status asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="Status">The status.</param>
        /// <param name="ttl">The TTL.</param>
        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus Status, TimeSpan? ttl = null)
        {
            var order = await GetOrderAsync(orderId);
            var noTTLOrder = await GetNoTTLOrderAsync(orderId);
            if (order != null)
            {
                order.Status = Status;
                var json = JsonSerializer.Serialize(order);
                await _database.StringSetAsync($"order:{orderId}", json, ttl ?? TimeSpan.FromMinutes(15));
                var noTTLjson = JsonSerializer.Serialize(noTTLOrder);
                await _database.StringSetAsync($"order_data:{orderId}", json);

                var expiration = DateTimeOffset.UtcNow.Add(ttl ?? TimeSpan.FromMinutes(15)).ToUnixTimeSeconds();
                await _database.SortedSetAddAsync(ExpiringOrdersKey, orderId.ToString(), expiration);
            }
        }
        /// <summary>
        /// Gets the no order without a TTL asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public async Task<OrderData?> GetNoTTLOrderAsync(Guid orderId)
        {
            var json = await _database.StringGetAsync($"order_data:{orderId}");
            if (json.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<OrderData>(json!);
        }

        /// <summary>
        /// Gets the order status asynchronously.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public async Task<OrderStatus?> GetOrderStatusAsync(Guid orderId)
        {
            var json = await _database.StringGetAsync($"order:{orderId}");
            if (json.IsNullOrEmpty) return null;

            using var doc = JsonDocument.Parse((string)json!);
            int enumValue = doc.RootElement.GetProperty("Status").GetInt32();
            return (OrderStatus)enumValue;
        }
        /// <summary>
        /// Gets the order asynchronously.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public async Task<OrderData?> GetOrderAsync(Guid orderId)
        {
            var json = await _database.StringGetAsync($"order:{orderId}");
            if (json.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<OrderData>(json!);
        }
        /// <summary>
        /// Removes the order asynchronous.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        public async Task RemoveOrderAsync(Guid orderId)
        {
            await _database.KeyDeleteAsync($"order:{orderId}");
            await _database.KeyDeleteAsync($"order_data:{orderId}");
            await _database.SortedSetRemoveAsync(ExpiringOrdersKey, orderId.ToString());
        }
        /// <summary>
        /// Gets the expired orders asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Guid>> GetExpiredOrdersAsync()
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var expired = await _database.SortedSetRangeByScoreAsync(ExpiringOrdersKey, stop: now);
            return expired.Select(x => Guid.Parse(x!));
        }
    }

}
