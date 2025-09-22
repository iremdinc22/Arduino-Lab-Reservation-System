using Microsoft.AspNetCore.Mvc;
using LabReservation.Data;
using LabReservation.Models;
using Microsoft.EntityFrameworkCore; // Db işlemleri için
using Microsoft.AspNetCore.Http; // Session işlemleri için

namespace LabReservation.Controllers
{
    public class AccountController : Controller
    {
        private readonly LabDbContext _context;

        public AccountController(LabDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string studentNumber, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.StudentNumber == studentNumber && u.Password == password);

            if (user != null)
            {
                // Giriş başarılı, oturumu aç
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserRole", user.Role); // Kullanıcının rolünü de sakla

                return RedirectToAction("Create", "Reservation");
            }

            // Giriş başarısız
            ViewBag.Error = "Student Number or Password is incorrect!";
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Oturumu temizler
            return RedirectToAction("Login", "Account");
        }
    }
}



