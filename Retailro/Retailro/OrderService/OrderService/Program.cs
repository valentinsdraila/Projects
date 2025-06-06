using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OrderService.DataLayer;
using OrderService.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrdersService>();
builder.Services.AddScoped<IProductInfoRepository, ProductInfoRepository>();
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddHostedService<EventsConsumer>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

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
