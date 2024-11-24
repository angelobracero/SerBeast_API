using SerBeast_API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class BookingCreateDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Booking date must be in the future.")]
        public DateTime BookingDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan BookingTime { get; set; }

        public int? ProfessionalServiceId { get; set; }
    }
}
