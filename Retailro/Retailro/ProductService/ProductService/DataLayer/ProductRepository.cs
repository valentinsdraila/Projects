using Microsoft.EntityFrameworkCore;
using ProductService.Model;
using ProductService.Model.Exceptions;
using System.Xml.Linq;

namespace ProductService.DataLayer
{
    public class ProductRepository : IProductRepository
    {
        private ProductDbContext context;
        public ProductRepository(ProductDbContext context)
        {
            this.context = context;
        }
        public async Task Add(Product product)
        {
            await context.AddAsync(product);
            await this.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            this.context.Set<Product>().Attach(product);
            this.context.Set<Product>().Remove(product);
            await this.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var entity = await this.GetById(id);
            if (entity != null)
            {
                this.context.Set<Product>().Remove(entity);
                await this.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetAll()
        {
            return await context.Set<Product>().ToListAsync();
        }

        public async Task<Product?> GetById(Guid id)
        {
            return await context.Set<Product>().FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                var savedChanges = await context.SaveChangesAsync();
                return savedChanges > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<List<Product>> SearchProducts(string query)
        {
            var results = await context.Set<Product>()
                .Select(p => new
                {
                    Product = p,
                    Score =
                        (p.Name.ToLower().Contains(query) ? 2 : 0) +
                        (p.Description.ToLower().Contains(query) ? 1 : 0)
                })
               .Where(p => p.Score > 0)
               .OrderByDescending(p => p.Score)
               .Select(p => new Product
               {
                   Id = p.Product.Id,
                   Name = p.Product.Name,
                   UnitPrice = p.Product.UnitPrice,
                   Image = p.Product.Image,
                   Quantity = p.Product.Quantity,
                   Description = p.Product.Description,
               })
               .ToListAsync();
            return results;
        }



        public async Task Update(Product product)
        {
            this.context.Entry<Product>(product).State = EntityState.Modified;
            await this.SaveChangesAsync();
        }
    }
}
