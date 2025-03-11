namespace ProductService.Model
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual List<Product>? Products { get; set; }
    }
}
