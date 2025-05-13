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
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddHostedService<EventsConsumer>();
builder.Services.AddHostedService<OrderExpirationService>();

builder.Services.AddSingleton<BraintreeGatewayFactory>();

var app = builder.Build();

var publisher = app.Services.GetRequiredService<RabbitMQPublisher>();
await publisher.InitializeAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
