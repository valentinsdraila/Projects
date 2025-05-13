namespace OrderService.Model
{
    /// <summary>
    /// Used for sending messages to the PaymentService.
    /// </summary>
    public class OrderCreatedMessage
    {
        public Guid OrderId { get; set; }
        public decimal Total {  get; set; }
        public OrderStatus Status { get; set; }
        public List<StockUpdate> StockUpdates { get; set; } = new List<StockUpdate>();
    }
}
