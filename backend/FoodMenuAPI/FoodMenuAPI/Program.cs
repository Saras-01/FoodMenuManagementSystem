using FoodMenuAPI.Data;
using Microsoft.EntityFrameworkCore;
using FoodMenuAPI.Repositories;
using FoodMenuAPI.Repositories.Interfaces;
using FoodMenuAPI.Services;
using FoodMenuAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register Repository
builder.Services.AddScoped<IFoodItemRepository, FoodItemRepository>();

// Register Service
builder.Services.AddScoped<IFoodItemService, FoodItemService>();

// 2. Register Entity Framework Core and SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
