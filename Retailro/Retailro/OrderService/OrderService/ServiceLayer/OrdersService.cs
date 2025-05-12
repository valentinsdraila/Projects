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

        public async Task<OrderDTO> AddOrder(List<ProductInfo> productInfos, Guid userId)
        {
            var totalPrice = productInfos.Sum(p => p.PriceAtPurchase * p.QuantityOrdered);
            var order = new Order()
            {
                CreatedAt = DateTime.Now,
                Status = OrderStatus.Processing,
                Products = productInfos,
                TotalPrice = totalPrice,
                UserId = userId,
                OrderNumber = await this.orderRepository.GetLastOrderNumber() + 1
            };

            await this.orderRepository.Add(order);
            var orderStockMessage = new OrderStockUpdateMessage { OrderId = order.Id };
            orderStockMessage.StockUpdates = order.Products.Select(p => new StockUpdate() { ProductId = p.ProductId, Quantity = p.QuantityOrdered, UnitPrice = p.PriceAtPurchase }).ToList();
            await rabbitMQPublisher.SendOrderCreated(new OrderCreatedMessage { OrderId = order.Id, Status = order.Status, Total = order.TotalPrice });
            await rabbitMQPublisher.SendStockUpdate(orderStockMessage);
            return new OrderDTO()
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice
            };
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
