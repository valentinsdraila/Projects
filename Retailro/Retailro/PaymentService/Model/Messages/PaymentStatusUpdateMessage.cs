namespace PaymentService.Model.Messages
{
    public class PaymentStatusUpdateMessage
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
