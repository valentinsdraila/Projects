
namespace OrderService.Model
{
    /// <summary>
    /// Order entity from the database.
    /// </summary>
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public int OrderNumber { get; set; }
        public OrderStatus Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public virtual List<ProductInfo> Products { get; set; } = new();
        public decimal TotalPrice { get; set; } 
    }

    public enum OrderStatus
    {
        None,
        Paid,
        Shipping,
        Completed,
        Cancelled,
        Valid,
        Processing
    }
}
