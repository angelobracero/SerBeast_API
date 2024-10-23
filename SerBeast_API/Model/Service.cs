using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        public string? ImageUrl { get; set; }

        public decimal? Price { get; set; }

        public string? Category { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string ProfessionalId { get; set; }

        [ForeignKey("ProfessionalId")]
        public virtual Professional Professional { get; set; }
    }
}
