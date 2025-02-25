using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzM4NCIsInR5cCI6IkpXVCJ9"))
        };
    });

var app = builder.Build();

// Middleware to extract JWT from cookie and add it to the Authorization header
app.Use(async (context, next) =>
{
    if (context.Request.Cookies.ContainsKey("jwt"))
    {
        var token = context.Request.Cookies["jwt"];
        context.Request.Headers.Add("Authorization", $"Bearer {token}");
    }

    await next();
});

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
await app.UseOcelot();
app.Run();
