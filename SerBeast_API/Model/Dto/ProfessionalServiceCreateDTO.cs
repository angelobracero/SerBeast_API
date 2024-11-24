using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class ProfessionalServiceCreateDTO
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        public string? Image { get; set; }

        [Required]
        public string ProfessionalId { get; set; }
    }
}
