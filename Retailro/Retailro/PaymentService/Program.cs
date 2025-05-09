using Braintree;
using PaymentService.Services;
using Microsoft.Extensions.Configuration;
using PaymentService.Controllers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();


builder.Services.Configure<BraintreeSettings>(builder.Configuration.GetSection("Braintree"));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<RedisService>();
builder.Services.AddHostedService<EventsConsumer>();

builder.Services.AddSingleton<BraintreeGatewayFactory>();
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
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowSpecificOrigin");

app.Run();
