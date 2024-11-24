using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class CategoryGetDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<ServiceGetDTO> Services { get; set; } = new List<ServiceGetDTO>();
    }
}
