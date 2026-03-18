using Microsoft.EntityFrameworkCore;
using DoAn.Models;

var builder = WebApplication.CreateBuilder(args);

// --- PHẦN 1: ĐĂNG KÝ DỊCH VỤ (SERVICES) ---

builder.Services.AddControllersWithViews();

// Đăng ký Database với SQL Server DESKTOP-LS4GB1H\SQLEXPRESS
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký dịch vụ Session (BẮT BUỘC nằm trước Build)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Phiên làm việc 30 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Xây dựng ứng dụng
var app = builder.Build();

// --- PHẦN 2: CẤU HÌNH PIPELINE (MIDDLEWARE) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Kích hoạt Session (Phải nằm sau UseRouting và trước MapControllerRoute)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();