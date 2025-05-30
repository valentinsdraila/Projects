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
                Brand = dto.Brand,
                Category = dto.Category,
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

        public async Task<List<string>> GetBrands()
        {
            return await _productRepository.GetBrands();
        }

        public async Task<List<string>> GetCategories()
        {
            return await _productRepository.GetCategories();
        }

        public async Task<ProductDto> GetProduct(Guid productId)
        {
            var product = await this._productRepository.GetById(productId);
            if (product != null)
            {
                return new ProductDto
                {
                    Id = product.Id,
                    Description = product.Description,
                    Image = product.Image,
                    Name = product.Name,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    Rating = new ProductRatingDto
                    {
                        AverageRating = product.Rating.AverageRating,
                        TotalReviews = product.Rating.TotalReviews
                    },
                    Reviews = product.Reviews.Select(r => new DisplayReviewDto
                    {
                        Id = r.Id,
                        ProductId = r.ProductId,
                        UserId = r.UserId,
                        Username = r.Username,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt
                    }).ToList()
                };
            }
            throw new Exception("Product not found!");
        }

        public async Task<List<ProductDto>> SearchProducts(string query, string category, string brand, decimal? minPrice, decimal? maxPrice, string sort)
        {
            query = query.ToLower();
            var products = _productRepository.SearchProducts(query, category, brand, minPrice, maxPrice);

            var productsList = products.ToList();
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
            return result.Select(p => new ProductDto()
            {
                Description = p.Description,
                Id = p.Id,
                Image = p.Image,
                Name = p.Name,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                Rating = new ProductRatingDto
                {
                    AverageRating = p.Rating.AverageRating,
                    TotalReviews = p.Rating.TotalReviews
                }
            }).ToList();
        }

        public async Task UpdateProduct(Product product)
        {
            await this._productRepository.Update(product);
        }
        public async Task<List<ProductDto>> GetNewest(int numberOfProducts = 4)
        {
            var products = await this._productRepository.GetNewest(numberOfProducts);
                
            return products.Select(p => new ProductDto()
            {
                Description = p.Description,
                Id = p.Id,
                Image = p.Image,
                Name = p.Name,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice,
                Rating = new ProductRatingDto
                {
                    AverageRating = p.Rating.AverageRating,
                    TotalReviews = p.Rating.TotalReviews
                }
            }).ToList(); ;
        }
    }
}
