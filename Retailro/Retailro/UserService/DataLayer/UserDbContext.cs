using Microsoft.EntityFrameworkCore;
using UserService.Model;
namespace UserService.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
       : base(options) { }
        DbSet<User> Users { get; set; }
        DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
    }
}