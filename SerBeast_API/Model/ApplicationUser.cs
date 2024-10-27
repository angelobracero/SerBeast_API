using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        public string? MiddleInitial { get; set; } 

        [Required]
        public string LastName { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string? HouseLotBlockNumber { get; set; } 

        public string? Street { get; set; } 

        public string? Barangay { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime? Birthday { get; set; }

        public string? Description { get; set; }

        public decimal? Rating { get; set; } = 0;

        public ICollection<ProfessionalService> ProfessionalServices { get; set; } = new List<ProfessionalService>();

        public ICollection<ServiceLocation> ServiceLocations { get; set; } = new List<ServiceLocation>();
    }

}
