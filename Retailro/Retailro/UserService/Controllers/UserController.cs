﻿using Microsoft.AspNetCore.Mvc;
using UserService.Service;
using UserService.Model;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns></returns>
        /*
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await this._userService.GetAllUsers();
            return Ok(users);
        }
        */
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Adds the user (register).
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddUser(User user)
        {
            try
            {
                bool userExists = await this._userService.AddUser(user);
                if (!userExists)
                {
                    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
                }
                else
                {
                    return Conflict(new {error = "UserExists", message = "The username/email is already in use!"});
                }
            }
            catch (ValidationException)
            {
                return BadRequest(new { error = "ValidationError", message = "The user data is invalid." });
            }
        }
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the current username and the role.
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        public IActionResult GetCurrentUsernameAndRole()
        {
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "JWT token missing" });
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token); 

            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            return Ok(new { username, role});
        }
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = Request.Headers["x-user-id"].FirstOrDefault();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in request." });
            }

            Guid userGuid = Guid.Parse(userId);
            var user = await _userService.GetUser(userGuid);
            return Ok(user);

        }
    }
}
