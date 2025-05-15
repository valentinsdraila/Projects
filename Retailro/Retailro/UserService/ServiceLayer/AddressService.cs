
using UserService.DataLayer;
using UserService.Model;

namespace UserService.ServiceLayer
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task AddAddress(DeliveryAddress address)
        {
            await _addressRepository.Add(address);
        }

        public async Task DeleteAddressById(Guid addressId)
        {
            await _addressRepository.Delete(addressId);
        }

        public async Task<DeliveryAddress> GetAddressById(Guid addressId)
        {
            return await _addressRepository.GetById(addressId);
        }

        public async Task<List<DeliveryAddress>> GetAddressesForUser(Guid userId)
        {
            return await _addressRepository.GetByUserId(userId);
        }
    }
}
