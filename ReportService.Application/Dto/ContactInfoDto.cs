namespace ReportService.Application.Dto
{
    public class ContactInfoDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public Guid HotelId { get; set; }
    }
}