using OrderService.Model;

namespace OrderService.ServiceLayer
{
    public interface IOrderService
    {
        /// <summary>
        /// Adds the order.
        /// </summary>
        /// <param name="productInfos">The product infos.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public Task<OrderDTO> AddOrder(AddOrderDTO addOrderDTO, Guid userId);
        /// <summary>
        /// Deletes the order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public Task DeleteOrder(Order order);
        /// <summary>
        /// Updates the order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public Task UpdateOrder(Order order);
        /// <summary>
        /// Gets the order by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The order</returns>
        public Task<Order> GetOrderById(Guid id);
        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A list of orders</returns>
        public Task<List<Order>> GetAllOrders();
        /// <summary>
        /// Gets all orders for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A list of OrderDTOs</returns>
        public Task<List<OrderDTO>> GetAllOrdersForUser(Guid userId);
        /// <summary>
        /// Deletes the order by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Task DeleteOrderById(Guid id);
    }
}
