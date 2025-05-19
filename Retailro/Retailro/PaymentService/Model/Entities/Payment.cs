using PaymentService.Model.Messages;

namespace PaymentService.Model.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date {  get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public PaymentStatus Status { get; set; }
    }
    public enum PaymentStatus
    {
        None,
        Completed,
        Cancelled
    }
}
