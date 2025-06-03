using Microsoft.EntityFrameworkCore;
using RecommendationService.Model;

namespace RecommendationService.DataLayer
{
    public class RecommendationDbContext : DbContext
    {
        public RecommendationDbContext(DbContextOptions options) : base(options)
        {
        }
        
        DbSet<UserProductInteraction> UserProductInteractions { get; set; }
    }
}
