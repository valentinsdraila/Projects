namespace UserService.Service;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Service class used for handling JWT tokens.
/// </summary>
public class JwtService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryMinutes;

    public JwtService(IConfiguration config)
    {
        _secret = config["JwtSettings:Secret"];
        _issuer = config["JwtSettings:Issuer"];
        _audience = config["JwtSettings:Audience"];
        _expiryMinutes = int.Parse(config["JwtSettings:ExpiryMinutes"]);
    }
    /// <summary>
    /// Generates the JWT token used for authentication.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="username">The username.</param>
    /// <param name="role">The role.</param>
    /// <returns></returns>
    public string GenerateToken(string userId, string username, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer, _audience, claims,
            expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    /// <summary>
    /// Validates the token.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secret);

        try
        {
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true
            };

            return tokenHandler.ValidateToken(token, validationParams, out _);
        }
        catch
        {
            return null;
        }
    }
}
