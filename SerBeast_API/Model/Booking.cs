using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SerBeast_API.Utilities;

namespace SerBeast_API.Model
{
    public class Booking
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        public int? ProfessionalServiceId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Booking date must be in the future.")]
        public DateTime BookingDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public string? CancellationReason { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ProfessionalServiceId")]
        public virtual ProfessionalService ProfessionalService { get; set; }
    }
}
