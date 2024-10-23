using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SerBeast_API.Utilities;

namespace SerBeast_API.Model
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public int? ServiceId { get; set; }

        public string? ProfessionalId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        [ForeignKey("ProfessionalId")]
        public virtual Professional Professional { get; set; }
    }
}
