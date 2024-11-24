using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SerBeast_API.Model.Dto
{
    public class ProfessionalServiceUpdateDTO
    {
        public int? CategoryId { get; set; }

        public int? ServiceId { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }
    }
}

