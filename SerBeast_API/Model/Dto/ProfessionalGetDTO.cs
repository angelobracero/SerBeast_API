using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class ProfessionalGetDTO
    {
        public string FirstName { get; set; }

        public string? MiddleInitial { get; set; }

        public string? Email { get; set; }

        public string LastName { get; set; }

        public string? HouseLotBlockNumber { get; set; }

        public string? Street { get; set; }

        public string? Barangay { get; set; }

        public string? Description { get; set; }

        public decimal? Rating { get; set; } = 0;

        public string? PhoneNumber { get; set; }

        public ICollection<ProfessionalServiceGetDTO> ProfessionalServices { get; set; } = new List<ProfessionalServiceGetDTO>();
    }
}
