using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Model
{
    [Owned]
    public class DeliveryAddress
    {
        [Required]
        [MinLength(3)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string County { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{5,10}$", ErrorMessage = "ZipCode must be between 5 and 10 digits.")]
        public string ZipCode { get; set; } = string.Empty;
    }

}
