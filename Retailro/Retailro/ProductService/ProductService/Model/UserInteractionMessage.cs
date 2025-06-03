namespace ProductService.Model
{
    public class UserInteractionMessage
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public InteractionType Action { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
    public enum InteractionType
    {
        View = 1,
        AddToCart = 3
    }
}
