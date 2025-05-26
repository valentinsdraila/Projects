using Microsoft.EntityFrameworkCore;
using ProductService.Model;

namespace ProductService.DataLayer
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductRating> ProductRating { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartId, ci.ProductId });

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Products)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Rating)
                .WithOne(r => r.Product)
                .HasForeignKey<ProductRating>(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
