using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SerBeast_API.Model;
using SerBeast_API.Utilities;

namespace SerBeast_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ServiceLocation> ServiceLocations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Professional>()
                .Property(p => p.Rating)
                .HasColumnType("decimal(4, 2)");

            // Configure Service
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(10, 2)"); 

            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion(
                    v => v.ToFriendlyString(),
                    v => (BookingStatus)Enum.Parse(typeof(BookingStatus), v));

        }
    }
}
