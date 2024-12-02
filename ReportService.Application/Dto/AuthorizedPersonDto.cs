namespace ReportService.Application.Dto
{
    public class AuthorizedPersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid HotelId { get; set; }
    }
}