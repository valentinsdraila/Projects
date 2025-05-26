using Microsoft.EntityFrameworkCore;
using ProductService.DataLayer;
using ProductService.Model;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace ProductService.ServiceLayer
{
    public class ProductsService : IProductService
    {
        private readonly IWebHostEnvironment _env;

        private readonly IProductRepository _productRepository;
        private readonly IProductRatingRepository _ratingRepository;

        public ProductsService(IProductRepository productRepository, IWebHostEnvironment env, IProductRatingRepository ratingRepository)
        {
            _productRepository = productRepository;
            _env = env;
            _ratingRepository = ratingRepository;
        }

        public async Task AddProduct(AddProductDto dto, IFormFile image)
        {
            string imageUrl = string.Empty;

            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var savePath = Path.Combine(_env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageUrl = $"{fileName}";
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Quantity = dto.Stock,
                UnitPrice = dto.Price,
                Image = imageUrl,
                CreatedAt = DateTime.UtcNow
            };
            await this._productRepository.Add(product);
            await this._ratingRepository.AddProductRating(new ProductRating { ProductId = product.Id, AverageRating = 0, TotalReviews = 0 });
        }

        public async Task DeleteProduct(Product product)
        {
            await this._productRepository.Delete(product);
        }

        public async Task DeleteProductById(Guid productId)
        {
            await this._productRepository.DeleteById(productId);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await this._productRepository.GetAll();
        }

        public async Task<Product?> GetProduct(Guid productId)
        {
            return await this._productRepository.GetById(productId);
        }

        public async Task<List<Product>> SearchProducts(string query, string category, string brand, decimal? minPrice, decimal? maxPrice, string sort)
        {
            query = query.ToLower();
            var products = _productRepository.SearchProducts(query, category, brand, minPrice, maxPrice);

            var productsList = await products.ToListAsync();
            var productIds = productsList.Select(p => p.Id).ToList();
            var numberOfReviewsList = await _ratingRepository.GetNumberOfReviews(productIds);

            products = sort switch
            {
                "price_asc" => products.OrderBy(p => p.UnitPrice),
                "price_desc" => products.OrderByDescending(p => p.UnitPrice),
                "newest" => products.OrderByDescending(p => p.CreatedAt),
                "nr_reviews" => products.OrderByDescending(p => numberOfReviewsList[p.Id]),
                _ => products
            };

            var result = products
                .ToList();
            return result;
        }

        public async Task UpdateProduct(Product product)
        {
            await this._productRepository.Update(product);
        }
    }
}
