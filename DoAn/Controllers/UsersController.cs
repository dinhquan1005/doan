using Microsoft.AspNetCore.Mvc;
using DoAn.Models;
using System.Linq;

namespace DoAn.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // 1. Giao diện Đăng ký
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges(); // Lệnh này đẩy dữ liệu xuống SQL Server
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // 2. Giao diện Đăng nhập
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string phone, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.PhoneNumber == phone && u.Password == password);
            if (user != null)
            {
                // LƯU ĐỦ 3 THÔNG TIN QUAN TRỌNG
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Role);

                // Chuyển hướng thông minh dựa trên vai trò
                if (user.Role == "Driver") return RedirectToAction("Index", "Trips");
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Sai số điện thoại hoặc mật khẩu!";
            return View();
        }
        public IActionResult Logout()
        {
            // Xóa toàn bộ thông tin đã lưu trong Session (UserName, UserID, UserRole)
            HttpContext.Session.Clear();

            // Sau khi đăng xuất, chuyển hướng người dùng về trang Login
            return RedirectToAction("Login", "Users");
        }
    }
}