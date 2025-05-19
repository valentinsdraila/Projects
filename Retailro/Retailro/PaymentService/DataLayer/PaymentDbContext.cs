using Microsoft.EntityFrameworkCore;
using PaymentService.Model.Entities;

namespace PaymentService.DataLayer
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions options) : base(options)
        {
        }
        DbSet<Payment> Payments { get; set; }
    }
}
