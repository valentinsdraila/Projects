namespace APIGateway;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class ExtractUserIdMiddleware
{
    private readonly RequestDelegate _next;

    public ExtractUserIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                context.Request.Headers.Add("x-user-id", userIdClaim.Value);
            }
        }

        await _next(context);
    }
}
