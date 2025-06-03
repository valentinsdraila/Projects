using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using RecommendationService.DataLayer;
using RecommendationService.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RecommendationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUserInteractionRepository, UserInteractionRepository>();
builder.Services.AddHostedService<UserInteractionConsumer>();
builder.Services.AddSingleton(new MLContext());
builder.Services.AddSingleton<ModelTrainer>();
builder.Services.AddSingleton<ModelManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


var manager = app.Services.GetRequiredService<ModelManager>();
await manager.TrainAndSaveModelAsync();

manager.LoadModel();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
