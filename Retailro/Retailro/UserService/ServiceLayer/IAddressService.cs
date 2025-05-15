using UserService.Model;

namespace UserService.ServiceLayer
{
    public interface IAddressService
    {
        Task AddAddress(DeliveryAddress address);
        Task DeleteAddressById(Guid addressId);
        Task<DeliveryAddress> GetAddressById(Guid addressId);
        Task<List<DeliveryAddress>> GetAddressesForUser(Guid userId);
    }
}
