using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class ProfessionalsGetDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }
                
        public string? MiddleInitial { get; set; }

        public string Email { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? Barangay { get; set; }

        public string? Description { get; set; }

        public decimal? Rating { get; set; } = 0;

        public string? PhoneNumber { get; set; }

        public List<ProfessionalServiceGetDTO> ProfessionalServices { get; internal set; }
    }
}
