using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerBeast_API.Model
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<ProfessionalService> ProfessionalServices { get; set; } = new List<ProfessionalService>(); 
    }
}
