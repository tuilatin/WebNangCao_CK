using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Trong Program.cs của Project Razor Pages

// Thêm đăng ký DbContext này vào services
builder.Services.AddDbContext<QUANLYBANHANG.Models.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// BẠN CŨNG CẦN ĐĂNG KÝ AppIdentityDbContext nếu bạn đang scaffolding Identity UI
builder.Services.AddDbContext<QUANLYBANHANG.Models.AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Đăng ký Named HttpClient
builder.Services.AddHttpClient("BackendApi", client =>
{
    // Đọc URL từ appsettings.json và thiết lập làm BaseAddress
    var baseUrl = builder.Configuration.GetValue<string>("ApiSettings:BackendUrl");
    client.BaseAddress = new Uri(baseUrl!);
});

// ...

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
