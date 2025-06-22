using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProductService.Model.Dtos
{
    /// <summary>
    /// Data transfer object used for displaying products for the user.
    /// </summary>
    public class CartItemDto
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        [NotNull]
        public int Quantity { get; set; }
        [Required]
        [NotNull]
        public decimal? TotalPrice { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
