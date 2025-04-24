using OrderService.Model;

namespace OrderService.DataLayer
{
    public interface IProductInfoRepository
    {
        Task Add(ProductInfo productInfo);
        Task<ProductInfo> GetById(Guid id);
        Task<List<ProductInfo>> GetAll();
        Task DeleteById(Guid id);
        Task Delete(ProductInfo productInfo);
        Task Update(ProductInfo productInfo);
        Task<bool> SaveChangesAsync();
    }
}
