using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ProductService.DataLayer;
using ProductService.Model;
using ProductService.Model.Dtos;
using ProductService.ServiceLayer;

namespace ProductServiceTests
{
    public class ProductServiceTests
    {
        private IProductRepository productMock;
        private IProductRatingRepository ratingMock;
        private IWebHostEnvironment envMock;
        public ProductServiceTests()
        {
            productMock = Substitute.For<IProductRepository>();
            ratingMock = Substitute.For<IProductRatingRepository>();
            envMock = Substitute.For<IWebHostEnvironment>();
        }
        [Fact]
        public async Task AddProduct_ShouldCallRepositoryFunction()
        {
            var image = Substitute.For<IFormFile>();
            ProductsService productService = new ProductsService(productMock, envMock, ratingMock);
            await productService.AddProduct(new AddProductDto(), image);
            await productMock.Received().Add(Arg.Any<Product>());
            await ratingMock.Received().AddProductRating(Arg.Any<ProductRating>());
        }
        [Fact]
        public async Task GetProduct_ShouldSuccessfullyBuildAProductDto()
        {
            var product = new Product
            {
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Brand = "Brand",
                CartItems = new List<CartItem>(),
                Description = "description",
                Category = "category",
                Image = "image",
                Name = "name",
                Quantity = 99,
                Rating = new ProductRating(),
                Reviews = new List<Review>(),
                UnitPrice = 999
            };
            var review = new Review()
            {
                CreatedAt = DateTime.UtcNow,
                Comment = "",
                Id = Guid.NewGuid(),
                Product = product,
                ProductId = product.Id,
                Rating = 5,
                UserId = Guid.NewGuid(),
                Username = ""
            };
            product.Reviews.Add(review);
            productMock.GetById(Arg.Any<Guid>()).Returns(product);
            ProductsService productService = new ProductsService(productMock, envMock, ratingMock);
            var returnedDto = await productService.GetProduct(product.Id);
            Assert.NotNull(returnedDto);
        }
        [Fact]
        public async Task GetProduct_ShouldThrowError_WhenTheProductIsNotFound()
        {
            productMock.GetById(Arg.Any<Guid>()).ReturnsNull();
            ProductsService productService = new ProductsService(productMock, envMock, ratingMock);
            await Assert.ThrowsAsync<Exception>(() => productService.GetProduct(new Guid()));
        }
    }
}
