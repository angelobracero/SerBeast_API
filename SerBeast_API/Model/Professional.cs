using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model
{
    public class Professional : ApplicationUser
    {
        [Required]
        public string ServiceType { get; set; }

        public string? Description { get; set; }

        public decimal Rating { get; set; } = 0;

        public ICollection<Service> Services { get; set; } = new List<Service>();

        public ICollection<ServiceLocation> ServiceLocations { get; set; } = new List<ServiceLocation>();
    }
}
