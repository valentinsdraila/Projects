namespace ProductService.Model
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<ProductDto> Products { get; set; } = new();
    }

}
