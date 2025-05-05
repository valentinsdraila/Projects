using System.ComponentModel.DataAnnotations;

namespace ProductService.Model
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? Image { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual List<CartItem>? CartItems { get; set; } = new();
    }
}
