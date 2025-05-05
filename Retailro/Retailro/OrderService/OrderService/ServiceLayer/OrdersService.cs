using OrderService.DataLayer;
using OrderService.Model;

namespace OrderService.ServiceLayer
{
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductInfoRepository productInfoRepository;
        private readonly RabbitMQPublisher rabbitMQPublisher;

        public OrdersService(IOrderRepository repository, IProductInfoRepository productInfoRepository, RabbitMQPublisher rabbitMQPublisher)
        {
            this.orderRepository = repository;
            this.productInfoRepository = productInfoRepository;
            this.rabbitMQPublisher = rabbitMQPublisher;
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
                OrderNumber = await this.orderRepository.GetLastOrderNumber() + 1
            };

            await this.orderRepository.Add(order);
            var orderStockMessagesList = new List<OrderStockUpdateMessage>();
            orderStockMessagesList = order.Products.Select(p => new OrderStockUpdateMessage() { ProductId = p.ProductId, Quantity = p.QuantityOrdered }).ToList();
            await rabbitMQPublisher.SendStockUpdate(orderStockMessagesList);
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
