using DoAn.Models;
using Microsoft.AspNetCore.Mvc;

public class TripsController : Controller
{
    private readonly ApplicationDbContext _context;
    public TripsController(ApplicationDbContext context) { _context = context; }

    // Dành cho Tài xế: Chỉ xem các chuyến "Pending"
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UserRole") != "Driver")
            return RedirectToAction("Login", "Users");

        var trips = _context.Trips.Where(t => t.Status == "Pending").OrderByDescending(t => t.CreatedAt).ToList();
        return View(trips);
    }

    // Hàm này để HIỂN THỊ cái trang web (Trình duyệt dùng GET)
    [HttpGet]
    public IActionResult Create()
    {
        // Kiểm tra session để đảm bảo chỉ Customer mới vào được
        if (HttpContext.Session.GetString("UserRole") != "Customer")
        {
            return RedirectToAction("Login", "Users");
        }
        return View();
    }

    // Hàm này để NHẬN DỮ LIỆU khi bạn nhấn nút Đặt xe (Form dùng POST)
    [HttpPost]
    public IActionResult Create(Trip trip)
    {
        var userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null) return RedirectToAction("Login", "Users");

        trip.CustomerID = userId.Value;
        trip.Status = "Pending";
        trip.CreatedAt = DateTime.Now;

        _context.Trips.Add(trip);
        _context.SaveChanges();
        return RedirectToAction("MyTrips");
    }

    // Trang danh sách chuyến đi của riêng Khách hàng đó
    public IActionResult MyTrips()
    {
        var userId = HttpContext.Session.GetInt32("UserID");
        if (userId == null) return RedirectToAction("Login", "Users");

        var myTrips = _context.Trips
            .Where(t => t.CustomerID == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();
        return View(myTrips);
    }
    [HttpPost]
    public IActionResult Finish(int id)
    {
        var trip = _context.Trips.Find(id);
        var driverId = HttpContext.Session.GetInt32("UserID");

        // Chỉ tài xế của chuyến đó mới được bấm hoàn thành
        if (trip != null && trip.DriverID == driverId)
        {
            trip.Status = "Completed";
            _context.SaveChanges();
        }
        return RedirectToAction("DriverHistory");
    }
    [HttpPost]
    public IActionResult Accept(int id)
    {
        // Lấy ID của tài xế đang đăng nhập từ Session
        var driverId = HttpContext.Session.GetInt32("UserID");

        if (driverId == null)
        {
            return RedirectToAction("Login", "Users");
        }

        var trip = _context.Trips.Find(id);
        if (trip != null && trip.Status == "Pending")
        {
            trip.DriverID = driverId;   // Gán tài xế nhận chuyến
            trip.Status = "Accepted";    // Đổi trạng thái
            _context.SaveChanges();      // Lưu vào SQL Server
        }

        return RedirectToAction("Index"); // Quay lại danh sách cuốc xe
    }

    [HttpPost]
    public IActionResult Cancel(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserID");
        var trip = _context.Trips.Find(id);

        if (trip != null && trip.CustomerID == userId && trip.Status == "Pending")
        {
            trip.Status = "Cancelled";
            _context.SaveChanges();
        }
        return RedirectToAction("MyTrips");
    }
    public IActionResult DriverHistory()
    {
        var driverId = HttpContext.Session.GetInt32("UserID");
        if (driverId == null || HttpContext.Session.GetString("UserRole") != "Driver")
            return RedirectToAction("Login", "Users");

        var history = _context.Trips
            .Where(t => t.DriverID == driverId)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();

        // Tính tổng thu nhập từ các chuyến 'Completed'
        ViewBag.TotalEarnings = history.Where(t => t.Status == "Completed").Sum(t => t.Price);

        return View(history);
    }
    [HttpPost]
    public IActionResult Complete(int id)
    {
        var driverId = HttpContext.Session.GetInt32("UserID");
        var trip = _context.Trips.Find(id);

        if (trip != null && trip.DriverID == driverId)
        {
            trip.Status = "Completed";
            _context.SaveChanges();
        }
        return RedirectToAction("DriverHistory");
    }
}