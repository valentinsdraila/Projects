using ProductService.Model;

namespace ProductService.DataLayer
{
    public interface IReviewRepository
    {
        /// <summary>
        /// Adds the review.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        Task AddReview(Review review);

        /// <summary>
        /// Gets the review.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        /// <returns></returns>
        Task<Review> GetReview(Guid reviewId);

        /// <summary>
        /// Gets all reviews.
        /// </summary>
        /// <returns></returns>
        Task<List<Review>> GetAllReviews();

        /// <summary>
        /// Gets all reviews for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<List<Review>> GetAllReviewsForUser(Guid userId);

        /// <summary>
        /// Gets all reviews for product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        Task<List<Review>> GetAllReviewsForProduct(Guid productId);

        /// <summary>
        /// Updates the review.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        Task UpdateReview(Review review);

        /// <summary>
        /// Deletes the review by identifier.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        /// <returns></returns>
        Task DeleteReviewById(Guid reviewId);

        /// <summary>
        /// Deletes the review.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        Task DeleteReview(Review review);
    }
}
