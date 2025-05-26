using System.ComponentModel.DataAnnotations;

namespace ProductService.Model
{
    public class ProductRating
    {
        [Key]
        public Guid ProductId { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Product Product { get; set; } = null!;
    }

}
