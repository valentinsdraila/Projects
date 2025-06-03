using System.ComponentModel.DataAnnotations;

namespace ProductService.Model.Dtos
{
    public class ReviewDto
    {
        public string Comment { get; set; } = string.Empty;
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
