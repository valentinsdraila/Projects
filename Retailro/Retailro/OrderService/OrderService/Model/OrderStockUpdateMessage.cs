namespace OrderService.Model
{
    /// <summary>
    /// Model class used for sending messages to the ProductService.
    /// </summary>
    public class OrderStockUpdateMessage
    {
        public Guid OrderId { get; set; }
        public List<StockUpdate> StockUpdates { get; set; } = new List<StockUpdate>();
    }
}
