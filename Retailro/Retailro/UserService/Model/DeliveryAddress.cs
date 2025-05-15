using System.ComponentModel.DataAnnotations;

namespace UserService.Model
{
    public class DeliveryAddress
    {
        public Guid Id { get; set; }
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
        public Guid UserId { get; set; }
        public User? User { get; set; }

    }
}
