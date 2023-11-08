using Booklist.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booklist.DataLayer
{
    public class EfDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBooks> UserBooks { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookImages> Images { get; set; }
        public EfDbContext()
        {
      //Data Source=localhost\\SQLEXPRESS
      //Server=localhost
      DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer("Data Source=localhost\\SQLEXPRESS; Database=Booklist; Trusted_Connection=True").Options;
        }
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.Development.json")
                   .Build();//Data Source=localhost\\SQLEXPRESS
                            //Server=localhost
        var connectionString = "Data Source=localhost\\SQLEXPRESS; Database=Booklist; Trusted_Connection=True";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
