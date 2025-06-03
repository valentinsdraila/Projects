namespace ProductService.Model.Dtos
{
    /// <summary>
    /// Data transfer object used for adding a product to the database.
    /// </summary>
    public class AddProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal? Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
    }
}
