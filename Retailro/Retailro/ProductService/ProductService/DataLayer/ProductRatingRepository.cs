using Microsoft.EntityFrameworkCore;
using ProductService.Model;

namespace ProductService.DataLayer
{
    public class ProductRatingRepository : IProductRatingRepository
    {
        private readonly ProductDbContext context;

        public ProductRatingRepository(ProductDbContext context)
        {
            this.context = context;
        }

        public async Task AddProductRating(ProductRating productRating)
        {
            await context.Set<ProductRating>().AddAsync(productRating);
            await context.SaveChangesAsync();
        }

        public async Task<ProductRating> GetProductRating(Guid productId)
        {
           return await context.Set<ProductRating>().FindAsync(productId) ?? new ProductRating();
        }

        public async Task UpdateProductRating(ProductRating productRating)
        {
            context.Entry<ProductRating>(productRating).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        public async Task<Dictionary<Guid, int>> GetNumberOfReviews(List<Guid> productIds)
        {
            return await context.Set<ProductRating>().Where(p => productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId, p => p.TotalReviews);
        }

    }
}
