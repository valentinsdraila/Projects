namespace ProductService.Model
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Image { get; set; }
        public List<DisplayReviewDto> Reviews { get; set; } = new();
        public ProductRatingDto Rating { get; set; } = new();
    }

}
