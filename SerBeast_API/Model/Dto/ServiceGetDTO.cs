using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class ServiceGetDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        public List<ServiceProfessionalServiceDTO> ProfessionalServices { get; set; }
    }
}
