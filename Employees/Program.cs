using Employees.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Добавление контекса в качестве сервиса
builder.Services.AddDbContext<ApplicationContext>(options =>options.UseSqlServer(connection));

// Создаёт один экземпляр сервиса на время работы приложения
//builder.Services.AddSingleton<IFileProvider>(
 //       new PhysicalFileProvider(
   //         Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files")));

builder.Services.AddLogging();
builder.Services.AddControllersWithViews();



var app = builder.Build();

app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();
