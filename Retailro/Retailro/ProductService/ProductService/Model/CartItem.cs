namespace ProductService.Model
{
    /// <summary>
    /// CartItem entity from the database.
    /// </summary>
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = new Product();
        public Guid CartId { get; set; }
        public virtual Cart Cart { get; set; } = new Cart();
        public int Quantity { get; set; }
    }

}
