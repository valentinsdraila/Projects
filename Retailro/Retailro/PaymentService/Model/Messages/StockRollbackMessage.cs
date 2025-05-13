namespace PaymentService.Model.Messages
{
    public class StockRollbackMessage
    {
        public Guid OrderId { get; set; }
        public List<StockUpdate> StockUpdates { get; set; } = new List<StockUpdate>();
    }
}
