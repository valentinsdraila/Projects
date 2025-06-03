using ProductService.DataLayer;
using ProductService.Model;
using ProductService.Model.Dtos;

namespace ProductService.ServiceLayer
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRatingRepository _ratingRepository;

        public ReviewService(IReviewRepository reviewRepository, IProductRatingRepository ratingRepository)
        {
            _reviewRepository = reviewRepository;
            _ratingRepository = ratingRepository;
        }

        public async Task AddReview(ReviewDto addReviewDto, Guid productId, Guid userId, string username)
        {
            var review = new Review
            {
                ProductId = productId,
                Comment = addReviewDto.Comment,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Rating = addReviewDto.Rating,
                Username = username,
            };
            var productRating = await _ratingRepository.GetProductRating(review.ProductId);
            productRating.TotalReviews += 1;
            var newRating = (productRating.AverageRating * (productRating.TotalReviews - 1) + review.Rating) / productRating.TotalReviews;
            productRating.AverageRating = newRating;
            await _ratingRepository.UpdateProductRating(productRating);
            await _reviewRepository.AddReview(review);
        }
        public async Task<Review> GetReview(Guid reviewId)
        {
            return await _reviewRepository.GetReview(reviewId);
        }
        public async Task DeleteReview(Review review)
        {
            await _reviewRepository.DeleteReview(review);
        }
        public async Task UpdateReview(Review review, ReviewDto reviewDto)
        {
            review.Rating = reviewDto.Rating;
            review.Comment = reviewDto.Comment;
            await _reviewRepository.UpdateReview(review);
        }
    }
}
