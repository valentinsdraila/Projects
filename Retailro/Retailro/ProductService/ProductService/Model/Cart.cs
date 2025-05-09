namespace ProductService.Model
{
    /// <summary>
    /// Cart entity from the database.
    /// </summary>
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual List<CartItem>? Products { get; set; } = new();
    }
}
