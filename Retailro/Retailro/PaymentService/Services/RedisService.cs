using PaymentService.Model;
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
        public async Task SetOrderStatusAsync(Guid orderId, OrderStatus Status, decimal Total, TimeSpan? ttl = null)
        {
            var orderStatus = new
            {
                Status,
                Total
            };

            var json = JsonSerializer.Serialize(orderStatus);
            await _database.StringSetAsync($"order:{orderId}", json, ttl ?? TimeSpan.FromMinutes(15));
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
    }

}
