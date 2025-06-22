using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProductService.Model.Dtos
{
    /// <summary>
    /// Data transfer object used for adding a product to the database.
    /// </summary>
    public class AddProductDto
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        [NotNull]
        public int Stock { get; set; }
        [Required]
        [NotNull]
        public decimal? Price { get; set; }
        [Required]
        [MinLength(2)]
        public string Category { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public string Brand { get; set; } = string.Empty;
    }
}
