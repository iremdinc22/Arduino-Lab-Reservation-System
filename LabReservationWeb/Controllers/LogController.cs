using Microsoft.AspNetCore.Mvc;
using LabReservation.Data; // DbContext namespace'in
using LabReservation.Models; // Log modelin bu namespace'te olmalı
using System.Threading.Tasks;

namespace LabReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LabDbContext _db;

        public LogController(LabDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> PostLog([FromBody] LogDto log)
        {
            if (string.IsNullOrEmpty(log.StudentNumber))
                return BadRequest("Student number required.");

            var newLog = new Log
            {
                StudentNumber = log.StudentNumber,
                Message = log.Message,
                Timestamp = DateTime.Now
            };

            _db.Logs.Add(newLog);
            await _db.SaveChangesAsync();

            return Ok(new { status = "Log saved." });
        }
    }
}


public class LogDto
{
    public string StudentNumber { get; set; }
    public string Message { get; set; }
}
