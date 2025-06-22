using Microsoft.AspNetCore.Http;
using NSubstitute;
using ProductService.DataLayer;
using ProductService.Model.Dtos;
using ProductService.Model;
using ProductService.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductServiceTests
{
    public class CartServiceTests
    {
        private ICartRepository cartMock;

        public CartServiceTests()
        {
            this.cartMock = Substitute.For<ICartRepository>();
        }
        [Fact]
        public async Task AddProductToCart_ShouldCallRepositoryFunction()
        {
            var productMock = Substitute.For<IProductService>();
            productMock.GetProduct(Arg.Any<Guid>()).Returns(new ProductDto());
            CartService cartService = new CartService(cartMock);
            await cartService.AddItemToCart(new Guid(), new Guid(), productMock);
            await cartMock.Received().Update(Arg.Any<Cart>());
        }
        [Fact]
        public async Task GetAllProductsInCart_ShouldReturnAListOfCartItemDtos()
        {
            cartMock.GetByUserId(Arg.Any<Guid>()).Returns(new Cart());
            CartService cartService = new CartService(cartMock);
            var returnedList = await cartService.GetProductsInCart(new Guid());
            Assert.IsType<List<CartItemDto>>(returnedList);
        }
    }
}
