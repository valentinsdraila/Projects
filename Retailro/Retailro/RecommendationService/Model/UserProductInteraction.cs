namespace RecommendationService.Model
{
    public class UserProductInteraction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public InteractionType Action { get; set; }
        public DateTime Timestamp { get; set; }

    }
    public enum InteractionType
    {
        View = 1,
        AddToCart = 3,
        Paid = 5
    }
}
