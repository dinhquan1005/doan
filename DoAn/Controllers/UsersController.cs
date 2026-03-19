using Microsoft.AspNetCore.Mvc;
using DoAn.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace DoAn.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db) { _db = db; }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng số điện thoại
                if (_db.Users.Any(u => u.PhoneNumber == user.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Số điện thoại đã được đăng ký.");
                    return View(user);
                }

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string phone, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.PhoneNumber == phone && u.Password == password);
            if (user != null)
            {
                // Tạo Claims chuẩn ASP.NET Core
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                if (user.Role == AppRoles.Driver) return RedirectToAction("Index", "Trips");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai số điện thoại hoặc mật khẩu!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login", "Users");
        }
    }
}