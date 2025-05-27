using Microsoft.AspNetCore.Mvc;
using ProductService.Model;
using ProductService.ServiceLayer;
using System.Data;
using System.Security.Claims;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("/api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost("product/{productId}")]
        public async Task<IActionResult> AddReview(ReviewDto addReviewDto, Guid productId)
        {
            try
            {
                var username = string.Empty;
                Guid userIdGuid = Guid.Empty;
                if (User != null)
                {
                    username = User.FindFirst(ClaimTypes.Name)?.Value;
                    bool success = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userIdGuid);
                    if (!success || username == null)
                    {
                        return Unauthorized(new { message = "The user Id/username could not be extracted!" });
                    }
                }
                try
                {
                    await _reviewService.AddReview(addReviewDto, productId, userIdGuid, username);

                    return Ok(new { message = "The review has been added!" });
                }
                catch (DBConcurrencyException ex)
                {
                    return BadRequest(new { message = "The review could not be added, try again!", error = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new {message="Error while trying to add the review!", error = ex.Message });
            }
        }
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

                if (!Guid.TryParse(userIdClaim, out Guid userId))
                    return Unauthorized(new { message = "Invalid user ID in token" });
                var review = await _reviewService.GetReview(reviewId);

                if (review.Username == string.Empty)
                    return NotFound();

                if (roleClaim != "Admin" && review.UserId != userId)
                    return Forbid();
                await _reviewService.DeleteReview(review);
                return Ok(new { message = "The review has been deleted successfuly!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while trying to update the review!", error = ex.Message });
            }
        }
        [HttpPatch("{reviewId}")]
        public async Task<IActionResult> UpdateReview(ReviewDto reviewDto, Guid reviewId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

                if (!Guid.TryParse(userIdClaim, out Guid userId))
                    return Unauthorized(new { message = "Invalid user ID in token" });
                var review = await _reviewService.GetReview(reviewId);

                if (review.Username == string.Empty)
                    return NotFound();

                if (roleClaim != "Admin" && review.UserId != userId)
                    return Forbid();
                await _reviewService.UpdateReview(review, reviewDto);
                return Ok(new { message = "The review has been deleted successfuly!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new {message="Error while trying to update the review!", error=ex.Message});
            }
        }
    }
}
