using OrderService.DataLayer;
using OrderService.Model;

namespace OrderService.ServiceLayer
{
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductInfoRepository productInfoRepository;

        public OrdersService(IOrderRepository repository, IProductInfoRepository productInfoRepository)
        {
            this.orderRepository = repository;
            this.productInfoRepository = productInfoRepository;
        }

        public async Task AddOrder(List<ProductInfo> productInfos, Guid userId)
        {
            var totalPrice = productInfos.Sum(p => p.PriceAtPurchase * p.QuantityOrdered);
            var order = new Order()
            {
                CreatedAt = DateTime.Now,
                Status = OrderStatus.Paid,
                Products = productInfos,
                TotalPrice = totalPrice,
                UserId = userId,
                OrderNumber = 1 //To be changed
            };
            await this.orderRepository.Add(order);
        }

        public async Task DeleteOrder(Order order)
        {
            await this.orderRepository.Delete(order);
        }

        public async Task DeleteOrderById(Guid id)
        {
            await this.orderRepository.DeleteById(id);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await this.orderRepository.GetAll();
        }
        public async Task<List<OrderDTO>> GetAllOrdersForUser(Guid userId)
        {
            var orders = await this.orderRepository.GetAllOrdersForUser(userId);
            return orders.Select(o => new OrderDTO()
            {
                OrderNumber = o.OrderNumber,
                CreatedAt = o.CreatedAt,
                Id = o.Id,
                Status = o.Status,
                TotalPrice = o.TotalPrice,
                UserId = o.UserId
            }).ToList();

        }
        public async Task<Order> GetOrderById(Guid id)
        {
            return await this.orderRepository.GetById(id);
        }

        public async Task UpdateOrder(Order order)
        {
            await this.orderRepository.Update(order);
        }
    }
}
