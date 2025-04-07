namespace OrderService.Model
{
    public class ProductInfo
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public decimal PriceAtPurchase { get; set; } 
        public string? Image { get; set; } 
        public int QuantityOrdered { get; set; }
        public virtual List<Order> Orders { get; set; } = new();
    }

}
