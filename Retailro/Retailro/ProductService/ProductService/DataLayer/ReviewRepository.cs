using Microsoft.EntityFrameworkCore;
using ProductService.Model;

namespace ProductService.DataLayer
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ProductDbContext context;

        public ReviewRepository(ProductDbContext context)
        {
            this.context = context;
        }

        public async Task AddReview(Review review)
        {
            await context.Set<Review>().AddAsync(review);
            await context.SaveChangesAsync();
        }

        public async Task DeleteReviewById(Guid reviewId)
        {
            var review = await context.Set<Review>().FindAsync(reviewId);
            if (review != null)
            {
                await this.DeleteReview(review);
            }
        }
        public async Task DeleteReview(Review review)
        {
            context.Set<Review>().Attach(review);
            context.Set<Review>().Remove(review);
            await context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetAllReviews()
        {
            return await context.Set<Review>().ToListAsync();
        }

        public async Task<List<Review>> GetAllReviewsForProduct(Guid productId)
        {
            return await context.Set<Review>().Where(r => r.ProductId == productId).ToListAsync();
        }

        public async Task<List<Review>> GetAllReviewsForUser(Guid userId)
        {
            return await context.Set<Review>().Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task<Review> GetReview(Guid reviewId)
        {
            return await context.Set<Review>().FindAsync(reviewId) ?? new Review();
        }

        public async Task UpdateReview(Review review)
        {
            context.Entry<Review>(review).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
