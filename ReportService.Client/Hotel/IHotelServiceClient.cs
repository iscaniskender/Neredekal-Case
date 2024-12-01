using App.Core.Results;
using ReportService.Client.Hotel.Model;

namespace ReportService.Client.Hotel;

public interface IHotelServiceClient
{
    Task<Result<HotelDto[]>> GetHotelByLocationAsync (string location);
}