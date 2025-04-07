using Microsoft.EntityFrameworkCore;
using OrderService.Model;

namespace OrderService.DataLayer
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository(OrderDbContext context)
        {
            this.context = context;
        }

        public OrderDbContext context { get; set; }
        public async Task Add(Order order)
        {
            await this.context.Set<Order>().AddAsync(order);
            await this.SaveChangesAsync();
        }

        public async Task Delete(Order order)
        {
            this.context.Set<Order>().Attach(order);
            this.context.Set<Order>().Remove(order);
            await this.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var order = await this.GetById(id);
            if(order!=null)
            {
                this.context.Set<Order>().Remove(order);
                await this.SaveChangesAsync();
            }
        }

        public async Task<List<Order?>> GetAll()
        {
            return await this.context.Set<Order>().ToListAsync();
        }

        public async Task<Order?> GetById(Guid id)
        {
            return await this.context.Set<Order>().FindAsync(id);
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

        public async Task Update(Order order)
        {
            this.context.Entry<Order>(order).State = EntityState.Modified;
            await this.SaveChangesAsync();
        }
    }
}
