namespace PaymentService.Model
{
    /// <summary>
    /// Represents the value of an order key in the redis database
    /// </summary>
    public class OrderData
    {
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
    }
}
