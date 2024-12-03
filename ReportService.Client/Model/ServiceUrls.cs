namespace ReportService.Client.Model;

public class ServiceUrls(string hotelServiceUrl)
{
    public string HotelServiceUrl { get; init; } = hotelServiceUrl;
}