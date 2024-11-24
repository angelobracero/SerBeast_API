namespace SerBeast_API.Model.Dto
{
    public class ProfessionalServiceGetDTO
    {
        public int ProfessionalServiceId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
