using LabReservation.Data;
using LabReservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LabReservation.Controllers
{
    public class MasterController : Controller
    {
        private readonly LabDbContext _context;

        public MasterController(LabDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Master")
                return Unauthorized();

            var reservations = await _context.Reservations
            .Include(r => r.Lab)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ThenByDescending(r => r.Id)
            .ToListAsync();


            return View(reservations); // Views/Master/All.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Lab)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Approved()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Master")
                return Unauthorized();

            var approvedReservations = await _context.Reservations
                .Include(r => r.Lab)
                .Include(r => r.User)
                .Where(r => r.Status == "Approved")
                .OrderByDescending(r => r.CreatedAt)
                .ThenByDescending(r => r.Id)
                .ToListAsync();

            return View(approvedReservations); // ✅ View'e veri gönder
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Lab)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation != null)
            {
                reservation.Status = "Approved";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("All");
        }


        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Lab)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation != null)
            {
                reservation.Status = "Rejected";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("All");
        }




    }
}
