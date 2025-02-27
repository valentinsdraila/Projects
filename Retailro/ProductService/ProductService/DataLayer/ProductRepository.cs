﻿using Microsoft.EntityFrameworkCore;
using ProductService.Model;
using System.Xml.Linq;

namespace ProductService.DataLayer
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository(ProductDbContext context)
        {
            this.context = context;
        }

        public ProductDbContext context { get; set; }
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

        public async Task Update(Product product)
        {
            this.context.Entry<Product>(product).State = EntityState.Modified;
            await this.SaveChangesAsync();
        }
    }
}
