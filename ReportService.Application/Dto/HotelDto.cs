using HotelService.Application.Dto;

namespace ReportService.Application.Dto
{
    public class HotelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ContactInfoDto> ContactInfos { get; set; } = new();
        public List<AuthorizedPersonDto> AuthorizedPersons { get; set; } = new();
    }
}

