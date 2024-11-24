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
        public DbSet<ProfessionalService> ProfessionalServices { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ServiceLocation> ServiceLocations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Home and Property Services"
                },
                new Category
                {
                    Id = 2,
                    Name = "Professional Services"
                },
                new Category
                {
                    Id = 3,
                    Name = "Personal Services"
                },
                new Category
                {
                    Id = 4,
                    Name = "Event Services"
                },
                new Category
                {
                    Id = 5,
                    Name = "Technology Services"
                },
                new Category
                {
                    Id = 6,
                    Name = "Automotive Services"
                },
                new Category
                {
                    Id = 7,
                    Name = "Specialized Services"
                });

            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Title = "Plumbing",
                    Description = "Repairs, installations, and emergency services.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 2,
                    Title = "Electrical Work",
                    Description = "Wiring, installations, and troubleshooting.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 3,
                    Title = "Cleaning Services",
                    Description = "Residential, commercial, deep cleaning, and move-in/move-out cleaning.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 4,
                    Title = "Landscaping",
                    Description = "Gardening, lawn care, and landscaping design.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 5,
                    Title = "Roofing",
                    Description = "Repairs, installations, and inspections.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 6,
                    Title = "HVAC Services",
                    Description = "Heating, ventilation, and air conditioning maintenance and installation.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 7,
                    Title = "Pest Control",
                    Description = "Extermination and prevention services.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 8,
                    Title = "Painting",
                    Description = "Interior and exterior painting services.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 9,
                    Title = "Carpentry",
                    Description = "Custom furniture, repairs, and installations.",
                    CategoryId = 1
                },
                new Service
                {
                    Id = 10,
                    Title = "Tutoring",
                    Description = "Academic tutoring and test preparation.",
                    CategoryId = 2
                },
                new Service
                {
                    Id = 11,
                    Title = "Consulting",
                    Description = "Business consulting for startups or management.",
                    CategoryId = 2
                },
                new Service
                {
                    Id = 12,
                    Title = "Legal Services",
                    Description = "Document preparation, legal advice, and representation.",
                    CategoryId = 2
                },
                new Service
                {
                    Id = 13,
                    Title = "Financial Services",
                    Description = "Accounting, bookkeeping, and financial planning.",
                    CategoryId = 2
                },
                new Service
                {
                    Id = 14,
                    Title = "Beauty Services",
                    Description = "Haircuts, styling, makeup, and skincare.",
                    CategoryId = 3
                },
                new Service
                {
                    Id = 15,
                    Title = "Fitness Training",
                    Description = "Personal training, yoga, and fitness classes.",
                    CategoryId = 3
                },
                new Service
                {
                    Id = 16,
                    Title = "Childcare",
                    Description = "Babysitting, nanny services, and daycare.",
                    CategoryId = 3
                },
                new Service
                {
                    Id = 17,
                    Title = "Catering",
                    Description = "Event catering for parties, weddings, and corporate events.",
                    CategoryId = 4
                },
                new Service
                {
                    Id = 18,
                    Title = "Photography",
                    Description = "Professional photography services for events and portraits.",
                    CategoryId = 4
                },
                new Service
                {
                    Id = 19,
                    Title = "Event Planning",
                    Description = "Planning and coordinating events.",
                    CategoryId = 4
                },
                new Service
                {
                    Id = 20,
                    Title = "IT Support",
                    Description = "Computer repairs, tech support, and network setup.",
                    CategoryId = 5
                },
                new Service
                {
                    Id = 21,
                    Title = "Web Development",
                    Description = "Website design, development, and maintenance.",
                    CategoryId = 5
                },
                new Service
                {
                    Id = 22,
                    Title = "Graphic Design",
                    Description = "Branding, logo design, and marketing materials.",
                    CategoryId = 5
                },
                new Service
                {
                    Id = 23,
                    Title = "Car Repairs",
                    Description = "General maintenance and repairs.",
                    CategoryId = 6
                },
                new Service
                {
                    Id = 24,
                    Title = "Detailing Services",
                    Description = "Car washing and detailing.",
                    CategoryId = 6
                },
                new Service
                {
                    Id = 25,
                    Title = "Pet Services",
                    Description = "Dog walking, pet sitting, and grooming.",
                    CategoryId = 7
                },
                new Service
                {
                    Id = 26,
                    Title = "Moving Services",
                    Description = "Packing, loading, and transporting belongings.",
                    CategoryId = 7
                },
                new Service
                {
                    Id = 27,
                    Title = "Interior Design",
                    Description = "Home staging and interior design consultations.",
                    CategoryId = 7
                });



            modelBuilder.Entity<ProfessionalService>(entity =>
            {
                entity.Property(s => s.Price)
                    .HasColumnType("decimal(10, 2)");

                entity.HasOne(ps => ps.Professional)
                    .WithMany(u => u.ProfessionalServices)
                    .HasForeignKey(ps => ps.ProfessionalId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ps => ps.Service)
                    .WithMany(s => s.ProfessionalServices)
                    .HasForeignKey(ps => ps.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ps => ps.Category)
                    .WithMany()
                    .HasForeignKey(ps => ps.CategoryId)
                    .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete
            });

            modelBuilder.Entity<Service>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();
        }
    }
}
