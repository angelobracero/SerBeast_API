using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class RegisterRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? MiddleInitial { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string HouseLotBlockNumber { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Barangay { get; set; }

        //[Required]
        //[Compare("Password", ErrorMessage = "Passwords do not match.")]
        //public string ConfirmPassword { get; set; }

        public string? Description { get; set; }
    }
}
