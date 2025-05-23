using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Service;

/// <summary>
/// Controller used in handling user authentication.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly IUserService _userService;

    public AuthController(JwtService jwtService, IUserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }
    /// <summary>
    /// Handles user login.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsersService.LoginRequest request)
    {
        Console.WriteLine("Login...");
        var user = await _userService.AuthenticateUser(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }
        var token = _jwtService.GenerateToken(user.Id.ToString(), user.Username, user.Role);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, 
            Secure=true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(60)
        };

        Response.Cookies.Append("jwt", token, cookieOptions);
        return Ok(new { message = "Logged in successfully" });
    }
    /// <summary>
    /// Handles the user logout.
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        Response.Cookies.Append("jwt", "", cookieOptions);
        return Ok(new { message = "Logged out successfully" });
    }

}
