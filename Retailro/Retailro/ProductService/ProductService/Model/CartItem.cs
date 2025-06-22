using System.ComponentModel.DataAnnotations;

namespace ProductService.Model
{
    /// <summary>
    /// CartItem entity from the database.
    /// </summary>
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public Guid CartId { get; set; }
        public virtual Cart? Cart { get; set; }
        [Required]
        public int Quantity { get; set; }
    }

}
