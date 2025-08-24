using Employees.Models.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ���������� �������� � �������� �������
builder.Services.AddDbContext<ApplicationContext>(options =>options.UseSqlServer(connection));


builder.Services.AddLogging();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();
