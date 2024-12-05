using App.Core.Results;
using HotelService.Application.Dto;
using MediatR;

namespace HotelService.Application.Hotel.Query;

public class GetHotelByLocationQuery:IRequest<Result<HotelDto[]>>
{
    public string Location { get; set; }

    public GetHotelByLocationQuery(string location)
    {
        Location = location;
    }
}