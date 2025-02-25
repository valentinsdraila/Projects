using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Service;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsersService.LoginRequest request)
    {
        Console.WriteLine("Login...");
        var user = await _userService.AuthenticateUser(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }
        var token = _jwtService.GenerateToken(user.Id.ToString(), user.Username, user.Role);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,  // Prevent XSS
            Secure=true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(60)
        };

        Response.Cookies.Append("jwt", token, cookieOptions);
        return Ok(new { message = "Logged in successfully" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok(new { message = "Logged out successfully" });
    }
}
