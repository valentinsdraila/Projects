using OrderService.Model;

namespace OrderService.ServiceLayer
{
    public interface IOrderService
    {
        public Task AddOrder(List<ProductInfo> productInfos, Guid userId);
        public Task DeleteOrder(Order order);
        public Task UpdateOrder(Order order);
        public Task<Order> GetOrderById(Guid id);
        public Task<List<Order>> GetAllOrders();
        public Task<List<OrderDTO>> GetAllOrdersForUser(Guid userId);
        public Task DeleteOrderById(Guid id);
    }
}
