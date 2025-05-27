using ProductService.Model;

namespace ProductService.ServiceLayer
{
    public interface IReviewService
    {
        /// <summary>
        /// Adds the review.
        /// </summary>
        /// <param name="addReviewDto">The add review dto.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        Task AddReview(ReviewDto addReviewDto, Guid productId, Guid userId, string username);

        /// <summary>
        /// Gets the review.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        /// <returns></returns>
        Task<Review> GetReview(Guid reviewId);

        /// <summary>
        /// Deletes the review.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        Task DeleteReview(Review review);

        /// <summary>
        /// Updates the review.
        /// </summary>
        /// <param name="review">The review.</param>
        /// <returns></returns>
        Task UpdateReview(Review review, ReviewDto reviewDto);
    }
}
