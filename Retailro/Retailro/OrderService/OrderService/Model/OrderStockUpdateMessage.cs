namespace OrderService.Model
{
    public class OrderStockUpdateMessage
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
