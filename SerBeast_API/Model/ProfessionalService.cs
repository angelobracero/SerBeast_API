using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model
{
    public class ProfessionalService
    {
        [Key]
        public int ProfessionalServiceId { get; set; }

        public int ServiceId { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public string ProfessionalId { get; set; }

        public decimal Price { get; set; } 

        public string Description { get; set; }

        public string? Image { get; set; }

        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [ForeignKey("ProfessionalId")]
        public virtual ApplicationUser Professional { get; set; }

    }
}
