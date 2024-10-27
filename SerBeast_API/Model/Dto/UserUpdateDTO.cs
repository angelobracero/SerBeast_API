using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class UserUpdateDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? MiddleInitial { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? HouseLotBlockNumber { get; set; }

        public string? Street { get; set; }

        public string? Barangay { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
