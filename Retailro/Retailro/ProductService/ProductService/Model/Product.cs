using System.ComponentModel.DataAnnotations;

namespace ProductService.Model
{
    /// <summary>
    /// Product entity from the database.
    /// </summary>
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Image { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual List<CartItem>? CartItems { get; set; } = new();
        public ProductRating? Rating { get; set; }
        public List<Review>? Reviews { get; set; }
    }
}
