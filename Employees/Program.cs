using Microsoft.EntityFrameworkCore;
using Employees.Models.Database;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ƒобавление контекса в качестве сервиса
builder.Services.AddDbContext<ApplicationContext>(options =>options.UseSqlServer(connection));

builder.Services.AddLogging();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();
