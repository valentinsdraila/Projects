using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Model;

namespace UserService.DataLayer
{
    public class AddressRepository : IAddressRepository
    {
        private readonly UserDbContext dbContext;

        public AddressRepository(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Add(DeliveryAddress address)
        {
            await dbContext.Set<DeliveryAddress>().AddAsync(address);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid addressId)
        {
            var address = await dbContext.Set<DeliveryAddress>().FindAsync(addressId);
            if (address != null)
            {
                dbContext.Set<DeliveryAddress>().Remove(address);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<DeliveryAddress> GetById(Guid addressId)
        {
           var address = await dbContext.Set<DeliveryAddress>().FindAsync(addressId);
            if (address != null)
                return address;
            return new DeliveryAddress();
        }

        public async Task<List<DeliveryAddress>> GetByUserId(Guid userId)
        {
            var addresses = await dbContext.Set<DeliveryAddress>()
                .Where(a => a.UserId == userId)
                .ToListAsync();
            if(addresses != null) return addresses;
            return new List<DeliveryAddress>();
        }
    }
}
