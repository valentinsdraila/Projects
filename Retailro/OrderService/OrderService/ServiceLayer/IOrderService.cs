using OrderService.Model;

namespace OrderService.ServiceLayer
{
    public interface IOrderService
    {
        public Task AddOrder(Order order);
        public Task DeleteOrder(Order order);
        public Task UpdateOrder(Order order);
        public Task<Order> GetOrderById(Guid id);
        public Task<List<Order>> GetAllOrders();
        public Task DeleteOrderById(Guid id);
    }
}
