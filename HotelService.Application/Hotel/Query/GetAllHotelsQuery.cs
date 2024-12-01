using MediatR;
using App.Core.Results;
using HotelService.Application.Dto;

namespace HotelService.Application.Hotel.Query
{
    public class GetAllHotelsQuery : IRequest<Result<HotelDto[]>> {}
    
}
