namespace ProductService.Model
{
    /// <summary>
    /// Data transfer object used for adding a product to the database.
    /// </summary>
    public class AddProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Stock {  get; set; }
        public decimal? Price { get; set; }
    }
}
