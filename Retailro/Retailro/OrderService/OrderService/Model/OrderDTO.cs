namespace OrderService.Model
{
    /// <summary>
    /// Used for adding or displaying an order.
    /// </summary>
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int OrderNumber { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
