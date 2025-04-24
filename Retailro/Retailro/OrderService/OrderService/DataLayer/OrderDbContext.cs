using Microsoft.EntityFrameworkCore;
using OrderService.Model;

namespace OrderService.DataLayer
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {
        }

        protected OrderDbContext()
        {
        }
        DbSet<Order> Orders { get; set; }
        DbSet<ProductInfo> ProductInfos { get; set; }
    }
}
