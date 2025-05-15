using UserService.Model;

namespace UserService.DataLayer
{
    public interface IAddressRepository
    {
        Task<List<DeliveryAddress>> GetByUserId(Guid userId);
        Task<DeliveryAddress> GetById(Guid addressId);
        Task Add(DeliveryAddress address);
        Task Delete(Guid addressId);
    }


}
