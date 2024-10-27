using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model
{
    public class ProfessionalService
    {
        [Key]
        public int ProfessionalServiceId { get; set; }

        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        public decimal Price { get; set; } 

        [Required]
        public string ProfessionalId { get; set; }

        [ForeignKey("ProfessionalId")]
        public virtual ApplicationUser Professional { get; set; }

    }
}
