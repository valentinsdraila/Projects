namespace ProductService.Model
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
