namespace PaymentService.Model.Messages
{
    public class StockUpdate
    {
        public Guid ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
