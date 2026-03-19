using DoAn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize] // Yêu cầu phải đăng nhập mới vào được Controller này
public class TripsController : Controller
{
    private readonly ApplicationDbContext _context;
    public TripsController(ApplicationDbContext context) { _context = context; }

    // Helper method lấy UserID hiện tại
    private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    [Authorize(Roles = AppRoles.Driver)]
    public IActionResult Index()
    {
        var trips = _context.Trips
            .Include(t => t.Customer) // Load sẵn tên khách hàng
            .Where(t => t.Status == TripStatus.Pending)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();
        return View(trips);
    }

    [Authorize(Roles = AppRoles.Customer)]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = AppRoles.Customer)]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Trip trip)
    {
        trip.CustomerID = GetCurrentUserId();
        trip.Status = TripStatus.Pending;
        trip.CreatedAt = DateTime.Now;

        _context.Trips.Add(trip);
        _context.SaveChanges();
        return RedirectToAction(nameof(MyTrips));
    }

    [Authorize(Roles = AppRoles.Customer)]
    public IActionResult MyTrips()
    {
        var myTrips = _context.Trips
            .Include(t => t.Driver)
            .Where(t => t.CustomerID == GetCurrentUserId())
            .OrderByDescending(t => t.CreatedAt)
            .ToList();
        return View(myTrips);
    }

    [Authorize(Roles = AppRoles.Driver)]
    public IActionResult DriverHistory()
    {
        var driverId = GetCurrentUserId();
        var history = _context.Trips
            .Where(t => t.DriverID == driverId)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();

        ViewBag.TotalEarnings = history.Where(t => t.Status == TripStatus.Completed).Sum(t => t.Price);
        return View(history);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Driver)]
    public IActionResult Accept(int id)
    {
        var trip = _context.Trips.Find(id);
        if (trip != null && trip.Status == TripStatus.Pending)
        {
            trip.DriverID = GetCurrentUserId();
            trip.Status = TripStatus.Accepted;
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Driver)]
    public IActionResult Complete(int id) // Đã gộp Finish và Complete lại thành 1
    {
        var trip = _context.Trips.Find(id);
        if (trip != null && trip.DriverID == GetCurrentUserId() && trip.Status == TripStatus.Accepted)
        {
            trip.Status = TripStatus.Completed;
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(DriverHistory));
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.Customer)]
    public IActionResult Cancel(int id)
    {
        var trip = _context.Trips.Find(id);
        if (trip != null && trip.CustomerID == GetCurrentUserId() && trip.Status == TripStatus.Pending)
        {
            trip.Status = TripStatus.Cancelled;
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(MyTrips));
    }
}