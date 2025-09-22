using Microsoft.EntityFrameworkCore;
using LabReservation.Models;

namespace LabReservation.Data
{
    public class LabDbContext : DbContext
    {
        public LabDbContext(DbContextOptions<LabDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Log> Logs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, StudentNumber = "2104014", Password = "12345", Role = "User" },
                new User { Id = 2, StudentNumber = "2102709", Password = "54321", Role = "User" },
                new User { Id = 3, StudentNumber = "2104248", Password = "33333", Role = "User" },
                new User { Id = 4, StudentNumber = "2104229", Password = "98765", Role = "User" },
                new User { Id = 5, StudentNumber = "2103094", Password = "98765", Role = "User" },
                new User { Id = 6, StudentNumber = "2103211", Password = "98765", Role = "User" },
                new User { Id = 7, StudentNumber = "2201085", Password = "98765", Role = "User" },
                new User { Id = 8, StudentNumber = "2102184", Password = "98765", Role = "User" },
                new User { Id = 9, StudentNumber = "2103575", Password = "98765", Role = "User" },
                new User { Id = 10, StudentNumber = "2202756", Password = "98765", Role = "User" },
                new User { Id = 11, StudentNumber = "9999999", Password = "admin", Role = "Master" }
            );

            modelBuilder.Entity<Lab>().HasData(
                new Lab { Id = 1, Name = "Delek1" },
                new Lab { Id = 2, Name = "Delek2" }
            );

            // ✅ Reservation → User ilişkilendirmesi
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            // ✅ Reservation → Lab ilişkilendirmesi (eğer eklenmediyse)
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Lab)
                .WithMany()
                .HasForeignKey(r => r.LabId);
        }

    }
}
