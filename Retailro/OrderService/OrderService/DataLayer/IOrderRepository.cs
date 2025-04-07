using OrderService.Model;

namespace OrderService.DataLayer
{
    public interface IOrderRepository
    {
        Task Add(Order order);
        Task<Order?> GetById(Guid id);
        Task<List<Order?>> GetAll();
        Task DeleteById(Guid id);
        Task Delete(Order order);
        Task Update(Order order);
        Task<bool> SaveChangesAsync();
    }
}
