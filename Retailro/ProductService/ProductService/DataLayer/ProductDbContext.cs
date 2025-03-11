using Microsoft.EntityFrameworkCore;
using ProductService.Model;

namespace ProductService.DataLayer
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        protected ProductDbContext()
        {
        }
        DbSet<Product> Products { get; set; }
        DbSet<Cart> Carts { get; set; }
    }
}
