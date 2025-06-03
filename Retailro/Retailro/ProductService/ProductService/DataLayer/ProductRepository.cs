using FuzzySharp;
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

        public async Task<List<string>> GetBrands()
        {
            return await context.Set<Product>()
                .Select(p => p.Brand)
                .Distinct()
                .ToListAsync();
        }

        public async Task<Product?> GetById(Guid id)
        {
            return await context.Set<Product>()
                .Include(p => p.Reviews)
                .Include(p => p.Rating)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<string>> GetCategories()
        {
            return await context.Set<Product>()
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();
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
        public IQueryable<Product> SearchProducts(string query, string category, string brand, decimal? minPrice, decimal? maxPrice)
        {
            var products = context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                var words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var productList = context.Products.Include(p => p.Rating).ToList();

                products = context.Products
                    .AsEnumerable()
                    .Select(p =>
                    {
                        var name = p.Name.ToLower();
                        var matchScores = words.Select(w => Fuzz.PartialRatio(w, name)).ToList();
                        var avgScore = matchScores.Average();
                        var strongMatches = matchScores.Count(s => s >= 75);

                        return new { Product = p, AvgScore = avgScore, StrongMatches = strongMatches };
                    })
                    .Where(r => r.StrongMatches >= 1 && r.AvgScore >= 50)
                    .OrderByDescending(r => r.AvgScore)
                    .Select(r => r.Product)
                    .AsQueryable();


            }
            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category.ToLower() == category.ToLower());

            if (!string.IsNullOrEmpty(brand))
                products = products.Where(p => p.Brand.ToLower() == brand.ToLower());

            if (minPrice.HasValue)
                products = products.Where(p => p.UnitPrice >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.UnitPrice <= maxPrice.Value);
            return products.Include(p => p.Rating);
        }

        public async Task Update(Product product)
        {
            this.context.Entry<Product>(product).State = EntityState.Modified;
            await this.SaveChangesAsync();
        }
        public async Task<List<Product>> GetNewest(int numberOfProducts)
        {
            return await context.Set<Product>()
                .OrderByDescending(p => p.CreatedAt)
                .Take(numberOfProducts)
                .Include(p => p.Rating)
                .ToListAsync();
        }
        public async Task<List<Product>> GetRecommended(List<Guid> productIds)
        {
            return await context.Set<Product>()
                .Where(p => productIds.Contains(p.Id))
                .Include(p => p.Rating)
                .ToListAsync();
        }
    }
}
