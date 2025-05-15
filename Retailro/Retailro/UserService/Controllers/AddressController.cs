using Microsoft.AspNetCore.Mvc;
using UserService.Model;
using UserService.ServiceLayer;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpPost]
        public async Task<IActionResult> AddCurrentUserAddress(DeliveryAddress address)
        {
            if (ModelState.IsValid)
            {
                var userId = Request.Headers["x-user-id"].FirstOrDefault();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in request." });
                }

                Guid userGuid = Guid.Parse(userId);
                address.UserId = userGuid;
                await _addressService.AddAddress(address);
                return Ok(address);
            }
            return BadRequest(new { message= "The delivery address is not valid!"});
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserAddresses()
        {
            var userId = Request.Headers["x-user-id"].FirstOrDefault();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in request." });
            }

            Guid userGuid = Guid.Parse(userId);
            var addresses = await _addressService.GetAddressesForUser(userGuid);
            return Ok(addresses);
        }
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            await _addressService.DeleteAddressById(addressId);
            return NoContent();
        }

    }
}
