using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProductService.Model
{
    /// <summary>
    /// Product entity from the database.
    /// </summary>
    public class Product
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        [MaxLength(2)]
        public string Category { get; set; } = string.Empty;
        [Required]
        [MaxLength(1)]
        public string Brand { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        [Required]
        [NotNull]
        public int Quantity { get; set; }
        [Required]
        [NotNull]
        public decimal? UnitPrice { get; set; }
        [Required]
        public string? Image { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual List<CartItem>? CartItems { get; set; } = new();
        public ProductRating? Rating { get; set; }
        public List<Review>? Reviews { get; set; }
    }
}
