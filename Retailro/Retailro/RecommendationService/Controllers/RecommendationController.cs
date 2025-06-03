using Microsoft.AspNetCore.Mvc;
using RecommendationService.ServiceLayer;

namespace RecommendationService.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    public class RecommendationController : ControllerBase
    {
        private readonly ModelManager modelManager;

        public RecommendationController(ModelManager modelManager)
        {
            this.modelManager = modelManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendations(int numberOfProducts = 4)
        {
            try
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }
                Guid userIdGuid = Guid.Parse(userId);
                var recommendations = await modelManager.PredictTopProductsForUser(userIdGuid, numberOfProducts);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error while trying to get recommendations ", error = ex.Message });
            }
        }
    }
}
