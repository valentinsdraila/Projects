using Microsoft.EntityFrameworkCore;
using ProductService.Model;

namespace ProductService.DataLayer
{
    public interface IProductRatingRepository
    {
        Task AddProductRating(ProductRating productRating);
        Task UpdateProductRating(ProductRating productRating);
        Task<ProductRating> GetProductRating(Guid productId);
        Task<Dictionary<Guid,int>> GetNumberOfReviews(List<Guid> productIds);

    }
}
