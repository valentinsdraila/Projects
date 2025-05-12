namespace PaymentService.Model
{
    public class PaymentStatusUpdateMessage
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
