using NSubstitute;
using ProductService.DataLayer;
using ProductService.Model;
using ProductService.Model.Dtos;
using ProductService.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductServiceTests
{
    public class ReviewServiceTests
    {
        private IReviewRepository reviewMock;
        private IProductRatingRepository productRatingMock;
        public ReviewServiceTests() {
            reviewMock = Substitute.For<IReviewRepository>();
            productRatingMock = Substitute.For<IProductRatingRepository>();
        }

        [Fact]
        public async Task AddReview_ShouldCallRepositoryFunctions()
        {
            productRatingMock.GetProductRating(Arg.Any<Guid>()).Returns(new ProductRating());
            var addReviewDto = new ReviewDto();
            var reviewService = new ReviewService(reviewMock, productRatingMock);
            await reviewService.AddReview(addReviewDto, new Guid(), new Guid(), string.Empty);
            await reviewMock.Received().AddReview(Arg.Any<Review>());
            await productRatingMock.Received().UpdateProductRating(Arg.Any<ProductRating>());
        }

    }
}
