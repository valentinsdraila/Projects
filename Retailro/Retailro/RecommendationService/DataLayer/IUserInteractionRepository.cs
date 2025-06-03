using RecommendationService.Model;

namespace RecommendationService.DataLayer
{
    public interface IUserInteractionRepository
    {
        Task Add(UserProductInteraction userInteraction);
        Task<List<UserProductInteraction>> GetAll();
        Task<List<Guid>> GetDistinctProductIds();
    }
}
