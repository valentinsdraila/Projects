namespace PaymentService.Model.Messages
{
    public class UserInteractionMessage
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public InteractionType Action { get; set; } = InteractionType.Paid;
        public DateTime Timestamp = DateTime.UtcNow;
    }

    public enum InteractionType
    {
        Paid = 5
    }
}
