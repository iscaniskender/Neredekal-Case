using HotelService.Application.Hotel.Dto;
using MediatR;
using App.Core.Results;

namespace HotelService.Application.Hotel.Query
{
    public class GetAllHotelsQuery : IRequest<Result<HotelDto[]>> {}
    
}
