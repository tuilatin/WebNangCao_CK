using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using QUANLYBANHANG.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm đăng ký DbContext này vào services
builder.Services.AddDbContext<QUANLYBANHANG.Models.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Đăng ký Named HttpClient
builder.Services.AddHttpClient("BackendApi", client =>
{
    // Đọc URL từ appsettings.json và thiết lập làm BaseAddress
    var baseUrl = builder.Configuration.GetValue<string>("ApiSettings:BackendUrl");
    client.BaseAddress = new Uri(baseUrl!);
});

// Thêm Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
