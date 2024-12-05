namespace ReportService.Client.Hotel.Model
{
    public class HotelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ContactInfoDto> ContactInfos { get; set; } = new();
        public List<AuthorizedPersonDto> AuthorizedPersons { get; set; } = new();
    }
}

