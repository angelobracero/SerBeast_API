using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerBeast_API.Model
{
    public class ServiceLocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Barangay { get; set; }

        [Required]
        public string ProfessionalId { get; set; }

        [ForeignKey("ProfessionalId")]
        public ApplicationUser Professional { get; set; }
    }
}
