using System;

namespace LabReservation.Models
{
        public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }           // Kullanıcıyla ilişki için
        public int LabId { get; set; }            // Laboratuvarla ilişki için
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = "Pending"; // default pending 


        // İlişkiler
        public User User { get; set; }            // Navigasyon property
        public Lab Lab { get; set; }              // Navigasyon property

        public DateTime CreatedAt { get; set; } 

    }

 }

