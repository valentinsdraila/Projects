using Microsoft.AspNetCore.Mvc;
using UserService.Model;
using UserService.ServiceLayer;

namespace UserService.Controllers
{
    /// <summary>
    /// Controller class managing delivery address functionality
    /// </summary>
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        /// <summary>
        /// Endpoint used to add an address for the logged in user
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Endpoint used for fetching all of a user's addresses from the database
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        ///  Endpoint used for deleting delivery addresses
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            await _addressService.DeleteAddressById(addressId);
            return NoContent();
        }

    }
}
