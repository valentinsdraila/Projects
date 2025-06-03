namespace RecommendationService.Model
{
    public class Mapper
    {
        public static uint ConvertGuidToUInt(Guid guid)
        {
            return BitConverter.ToUInt32(guid.ToByteArray(), 0);
        }

        public static IEnumerable<UserInteraction> MapToTrainerInput(IEnumerable<UserProductInteraction> entities)
        {
            return entities.Select(e => new UserInteraction
            {
                UserId = ConvertGuidToUInt(e.UserId),
                ProductId = ConvertGuidToUInt(e.ProductId),
                Label = e.Action switch
                {
                    InteractionType.View => 1f,
                    InteractionType.AddToCart => 3f,
                    InteractionType.Paid => 5f,
                    _ => 0f
                }
            });
        }

    }
}
