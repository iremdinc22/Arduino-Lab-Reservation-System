using Microsoft.AspNetCore.Mvc;
using LabReservation.Data;
using LabReservation.Models;
using Microsoft.EntityFrameworkCore;

namespace LabReservation.Controllers
{
    public class ReservationController : Controller
    {
        private readonly LabDbContext _context;

        public ReservationController(LabDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            ViewBag.Labs = _context.Labs.ToList();

            if (userId != null)
            {
                var reservations = _context.Reservations
                    .Include(r => r.Lab)
                    .Where(r => r.UserId == userId)
                    .ToList(); // önce veritabanından çekiyoruz (LINQ to Entities BİTER)

                ViewBag.MyReservations = reservations
                    .OrderByDescending(r => r.Date)
                    .ThenBy(r => r.StartTime)
                    .ToList(); // burada artık LINQ to Objects ile bellekte sıralama yapılıyor
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized();

            reservation.UserId = userId.Value;
            reservation.CreatedAt = DateTime.Now; // 👈 BU çok önemli

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync(); // 👈 BU satırdan sonra veri DB’ye yazılır

            TempData["Success"] = "Reservation added successfully.";
            return RedirectToAction("Create"); // 
        }


    }
}
