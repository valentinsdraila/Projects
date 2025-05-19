using ProductService.DataLayer;
using ProductService.Model;
using static System.Net.Mime.MediaTypeNames;

namespace ProductService.ServiceLayer
{
    public class ProductsService : IProductService
    {
        private readonly IWebHostEnvironment _env;

        private readonly IProductRepository _productRepository;

        public ProductsService(IProductRepository productRepository, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _env = env;
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
                Name = dto.Name,
                Description = dto.Description,
                Quantity = dto.Stock,
                UnitPrice = dto.Price,
                Image = imageUrl
            };
            await this._productRepository.Add(product);
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

        public async Task UpdateProduct(Product product)
        {
            await this._productRepository.Update(product);
        }
    }
}
