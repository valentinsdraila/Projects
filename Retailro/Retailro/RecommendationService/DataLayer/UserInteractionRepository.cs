using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using RecommendationService.Model;

namespace RecommendationService.DataLayer
{
    public class UserInteractionRepository : IUserInteractionRepository
    {
        private readonly RecommendationDbContext context;

        public UserInteractionRepository(RecommendationDbContext context)
        {
            this.context = context;
        }

        public async Task Add(UserProductInteraction userInteraction)
        {
            await context.Set<UserProductInteraction>().AddAsync(userInteraction);
            await context.SaveChangesAsync();
        }

        public async Task<List<UserProductInteraction>> GetAll()
        {
            return await context.Set<UserProductInteraction>().ToListAsync();
        }
        public async Task<List<Guid>> GetDistinctProductIds()
        {
            return await context.Set<UserProductInteraction>()
                .Select(ui => ui.ProductId)
                .Distinct()
                .ToListAsync();
        }

    }
}
