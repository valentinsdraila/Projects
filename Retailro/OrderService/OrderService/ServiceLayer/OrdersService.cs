using OrderService.DataLayer;
using OrderService.Model;

namespace OrderService.ServiceLayer
{
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository repository;

        public OrdersService(IOrderRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddOrder(Order order)
        {
            await this.repository.Add(order);
        }

        public async Task DeleteOrder(Order order)
        {
            await this.repository.Delete(order);
        }

        public async Task DeleteOrderById(Guid id)
        {
            await this.repository.DeleteById(id);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await this.repository.GetAll();
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            return await this.repository.GetById(id);
        }

        public async Task UpdateOrder(Order order)
        {
            await this.repository.Update(order);
        }
    }
}
