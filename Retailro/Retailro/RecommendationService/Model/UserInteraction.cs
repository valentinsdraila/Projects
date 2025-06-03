using Microsoft.ML.Data;

namespace RecommendationService.Model
{
    public class UserInteraction
    {
        [LoadColumn(0)] public uint UserId;
        [LoadColumn(1)] public uint ProductId;
        [LoadColumn(2)] public float Label;
    }
}
