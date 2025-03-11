using ProductService.Model;

namespace ProductService.DataLayer
{
    public interface ICartRepository
    {
        Task<Cart?> GetById(Guid id);
        Task<Cart?> GetByUserId(Guid userId);
        Task<List<Cart>> GetAll();
        Task Add(Cart cart);
        Task Update(Cart cart);
        Task DeleteById(Guid id);
        Task DeleteByUserId(Guid userId);
        Task Delete(Cart cart);
        Task<bool> SaveChangesAsync();
    }
}
