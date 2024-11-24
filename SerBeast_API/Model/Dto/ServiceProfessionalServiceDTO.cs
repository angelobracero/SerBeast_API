namespace SerBeast_API.Model.Dto
{
    public class ServiceProfessionalServiceDTO
    {
        public int ProfessionalServiceId { get; internal set; }
        public decimal Price { get; internal set; }
        public string ProfessionalId { get; internal set; }
        public string? Barangay { get; internal set; }
        public string ProfessionalName { get; internal set; }
        public string? Description { get; internal set; }
        public decimal Rating { get; internal set; }
        public string? PhoneNumber { get; set; }

    }
}
