namespace ReportService.Application.Dto;

public class ReportDto
{
    public Guid Id { get; set; }

    public string Status { get; set; }

    public string Location { get; set; } = string.Empty;

    public int HotelCount { get; set; }

    public int PhoneNumberCount { get; set; }

    public string CreatedAt { get; set; }
    
    public string UpdatedAt { get; set; }

    public bool IsActive { get; set; }
}