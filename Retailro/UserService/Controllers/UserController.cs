using Microsoft.AspNetCore.Mvc;
using UserService.Service;
using UserService.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await this._userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await this._userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(User user)
        {
            try
            {
                await this._userService.AddUser(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (ValidationException)
            {
                return BadRequest("The user data is invalid.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, User user)
        {
            await this._userService.UpdateUser(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            await this._userService.DeleteUserById(id);
            return NoContent();
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            // Get the JWT from the HTTP-only cookie
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "JWT token missing" });
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token); // Decode JWT

            // Retrieve the username using ClaimTypes.Name
            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            return Ok(new { username });
        }
    }
}
