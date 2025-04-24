using Microsoft.EntityFrameworkCore;
using OrderService.Model;

namespace OrderService.DataLayer
{
    public class ProductInfoRepository : IProductInfoRepository
    {
        private readonly OrderDbContext context;
        public ProductInfoRepository(OrderDbContext context)
        {
            this.context = context;
        }
        public async Task Add(ProductInfo productInfo)
        {
            await this.context.Set<ProductInfo>().AddAsync(productInfo);
            await this.context.SaveChangesAsync();
        }

        public async Task Delete(ProductInfo productInfo)
        {
            this.context.Set<ProductInfo>().Attach(productInfo);
            this.context.Set<ProductInfo>().Remove(productInfo);
            await this.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var productInfo = await this.GetById(id);
            if (productInfo != null)
            {
                this.context.Set<ProductInfo>().Remove(productInfo);
                await this.SaveChangesAsync();
            }
        }

        public async Task<List<ProductInfo>> GetAll()
        {
            return await context.Set<ProductInfo>().ToListAsync();
        }

        public async Task<ProductInfo> GetById(Guid id)
        {
            return await context.Set<ProductInfo>().FindAsync(id) ?? new ProductInfo();
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

        public async Task Update(ProductInfo productInfo)
        {
            this.context.Entry<ProductInfo>(productInfo).State = EntityState.Modified;
            await this.SaveChangesAsync();
        }
    }
}
