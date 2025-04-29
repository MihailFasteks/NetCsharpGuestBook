using Microsoft.EntityFrameworkCore;
using NetCsharpGuestBook.Models;

var builder = WebApplication.CreateBuilder(args);

// ��� ������ �������� ������ ������� IDistributedCache, � ASP.NET Core 
// ������������� ���������� ���������� IDistributedCache
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // ������������ ������ (����-��� ���������� ������)
    options.Cookie.Name = "Session"; // ������ ������ ����� ���� �������������, ������� ����������� � �����.

});
builder.Services.AddDbContext<BookContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),new MySqlServerVersion(new Version(9, 3, 0))));
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();   // ��������� middleware-��������� ��� ������ � ��������

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
