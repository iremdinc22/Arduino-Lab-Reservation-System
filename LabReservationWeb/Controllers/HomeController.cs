using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LabReservation.Models;

namespace LabReservation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // Kullanıcı login değil, Login sayfasına yönlendir
                return RedirectToAction("Login", "Account");
            }
            else
            {
                // Kullanıcı login olmuş, Reservation sayfasına yönlendir
                return RedirectToAction("Create", "Reservation");
            }
        }
    }
}

