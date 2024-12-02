namespace SerBeast_API.Model.Dto
{
    public class ProfessionalInfoDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? HouseLotBlockNumber { get; set; }
        public string? Street { get; set; }
        public string Barangay { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
    }

}
