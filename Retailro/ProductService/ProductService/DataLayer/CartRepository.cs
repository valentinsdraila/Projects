using Microsoft.EntityFrameworkCore;
using ProductService.Model;

namespace ProductService.DataLayer
{
    public class CartRepository : ICartRepository
    {
        public ProductDbContext context { get; set; }

        public CartRepository(ProductDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Cart cart)
        {
            await context.AddAsync(cart);
            await this.SaveChangesAsync();
        }

        public async Task Delete(Cart cart)
        {
            this.context.Set<Cart>().Attach(cart);
            this.context.Set<Cart>().Remove(cart);
            await this.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var entity = await this.GetById(id);
            if (entity != null)
            {
                this.context.Set<Cart>().Remove(entity);
                await this.SaveChangesAsync();
            }
        }

        public async Task<List<Cart>> GetAll()
        {
            return await context.Set<Cart>().ToListAsync();
        }

        public async Task<Cart?> GetById(Guid id)
        {
            return await context.Set<Cart>().FindAsync(id);
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

        public async Task Update(Cart cart)
        {
            this.context.Entry<Cart>(cart).State = EntityState.Modified;
            await this.SaveChangesAsync();
        }

        public async Task<Cart?> GetByUserId(Guid userId)
        {
            
            Cart cart = await context.Set<Cart>().FirstOrDefaultAsync(c=>c.UserId==userId);
            await context.Entry(cart).Collection(c => c.Products).LoadAsync();
            return cart;
        }

        public async Task DeleteByUserId(Guid userId)
        {
            var entity = await this.GetByUserId(userId);
            if (entity != null)
            {
                this.context.Set<Cart>().Remove(entity);
                await this.SaveChangesAsync();
            }
        }
    }
}
