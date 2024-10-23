using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Service> Services { get; set; } = new List<Service>();

    }
}
