using Microsoft.AspNetCore.Mvc;
using LabReservation.Data;
using Microsoft.EntityFrameworkCore;

namespace LabReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationApiController : ControllerBase
    {
        private readonly LabDbContext _db;

        public ReservationApiController(LabDbContext db)
        {
            _db = db;
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedReservations()
        {
            var approvedReservations = await _db.Reservations
                .Include(r => r.Lab)
                .Include(r => r.User)
                .Where(r => r.Status == "Approved")
                .ToListAsync();

            var result = approvedReservations.Select(r => new
            {
                StudentNumber = r.User?.StudentNumber,
                Password = r.User?.Password,
                Lab = r.Lab?.Name,
                Date = r.Date.ToString("yyyy-MM-dd"),
                StartTime = r.StartTime.ToString(@"hh\:mm"),
                EndTime = r.EndTime.ToString(@"hh\:mm"),
            });

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllReservations(
            string? status,
            string? startDate,
            string? endDate,
            string? startTime,
            string? endTime)
        {
            var query = _db.Reservations
                .Include(r => r.Lab)
                .Include(r => r.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);

            if (DateTime.TryParse(startDate, out DateTime startD))
                query = query.Where(r => r.Date >= startD);

            if (DateTime.TryParse(endDate, out DateTime endD))
                query = query.Where(r => r.Date <= endD);

            if (TimeSpan.TryParse(startTime, out TimeSpan startT))
                query = query.Where(r => r.StartTime >= startT);

            if (TimeSpan.TryParse(endTime, out TimeSpan endT))
                query = query.Where(r => r.EndTime <= endT);

            var reservations = await query.ToListAsync();

            var sorted = reservations
                .OrderByDescending(r => r.Date)
                .ThenBy(r => r.StartTime)
                .ToList();

            var result = sorted.Select(r => new
            {
                Id = r.Id,
                StudentNumber = r.User?.StudentNumber,
                Lab = r.Lab?.Name,
                Date = r.Date.ToString("yyyy-MM-dd"),
                StartTime = r.StartTime.ToString(@"hh\:mm"),
                EndTime = r.EndTime.ToString(@"hh\:mm"),
                Status = r.Status ?? "Pending"
            });

            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult LoginCheck([FromBody] LoginRequest request)
        {
            var user = _db.Users.FirstOrDefault(u =>
                u.StudentNumber == request.StudentNumber &&
                u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Öğrenci numarası veya şifre hatalı."
                });
            }

            var now = DateTime.Now;
            var today = now.Date;
            var currentTime = now.TimeOfDay;

            var reservation = _db.Reservations
                .Include(r => r.Lab)
                .FirstOrDefault(r =>
                    r.UserId == user.Id &&
                    r.Status == "Approved" &&
                    r.Date == today &&
                    r.StartTime <= currentTime &&
                    r.EndTime >= currentTime);

            if (reservation == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Şu anda geçerli bir onaylı rezervasyon bulunamadı."
                });
            }

            return Ok(new
            {
                success = true,
                studentNumber = user.StudentNumber,
                lab = reservation.Lab?.Name,
                date = reservation.Date.ToString("yyyy-MM-dd"),
                startTime = reservation.StartTime.ToString(@"hh\:mm"),
                endTime = reservation.EndTime.ToString(@"hh\:mm"),
                serverTime = now.ToString("yyyy-MM-dd HH:mm")
            });
        }


        // ✅ ARDUINO İÇİN RAM-DOSTU SADE ENDPOINT
        [HttpGet("check/{studentNumber}/{password}")]
        public IActionResult CheckSimple(string studentNumber, string password)
        {
            var user = _db.Users.FirstOrDefault(u =>
                u.StudentNumber == studentNumber &&
                u.Password == password);

            if (user == null)
                return Content("0", "text/plain"); // Öğrenci bulunamadı

            var now = DateTime.Now;
            var today = now.Date;
            var currentTime = now.TimeOfDay;

            var todayReservations = _db.Reservations
                .Where(r =>
                    r.UserId == user.Id &&
                    r.Status == "Approved" &&
                    r.Date == today)
                .ToList(); // Veriyi belleğe al

            var hasValidReservation = todayReservations
                .Any(r =>
                    r.StartTime <= currentTime &&
                    r.EndTime >= currentTime);

            return Content(hasValidReservation ? "1" : "0", "text/plain");
        }



        /* [HttpGet("check/{studentNumber}/{password}")]
          public IActionResult CheckSimple(string studentNumber, string password)
          {
              var user = _db.Users.FirstOrDefault(u =>
                  u.StudentNumber == studentNumber &&
                  u.Password == password);

              if (user == null)
                  return Content("0"); // Öğrenci bulunamadı

              var now = DateTime.Now;
              var today = now.Date;
              var currentTime = now.TimeOfDay;

              // EF Core'ın anlayabildiği kısmı DB'den çek
              var todaysReservations = _db.Reservations
                  .Where(r =>
                      r.UserId == user.Id &&
                      r.Status == "Approved" &&
                      r.Date == today)
                  .ToList(); // Veriyi belleğe al

              // Şu anki saat aralığında olanı bellekte kontrol et
              var hasValidReservation = todaysReservations.Any(r =>
                  r.StartTime <= currentTime &&
                  r.EndTime >= currentTime);

              return Content(hasValidReservation ? "1" : "0");
          } */




        /* [HttpGet("check-plain/{studentNumber}/{password}")]
public IActionResult CheckPlain(string studentNumber, string password)
{
    var user = _db.Users.FirstOrDefault(u =>
        u.StudentNumber == studentNumber &&
        u.Password == password);

    if (user == null)
        return Content("0"); // Kullanıcı yok

    var now = DateTime.Now;
    var today = now.Date;
    var currentTime = now.TimeOfDay;

    var hasValidReservation = _db.Reservations
        .Where(r =>
            r.UserId == user.Id &&
            r.Status == "Approved" &&
            r.Date == today)
        .ToList()
        .Any(r =>
            r.StartTime <= currentTime &&
            r.EndTime >= currentTime);

    return Content(hasValidReservation ? "1" : "0");
}
 */







        public class LoginRequest
        {
            public string StudentNumber { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
