using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class LoginRequestDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
