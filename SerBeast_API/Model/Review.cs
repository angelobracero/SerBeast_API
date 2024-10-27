using SerBeast_API.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Review
{
    public int Id { get; set; }

    [Required]
    public string ProfessionalId { get; set; } 

    [ForeignKey("ProfessionalId")]
    public ApplicationUser Professional { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; } 

    public string? Comments { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
