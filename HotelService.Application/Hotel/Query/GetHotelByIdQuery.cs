using App.Core.Results;
using HotelService.Application.Hotel.Dto;
using MediatR;

namespace HotelService.Application.Hotel.Query
{
    public class GetHotelByIdQuery : IRequest<Result<HotelDto>>
    {
        public Guid Id { get; set; }

        public GetHotelByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
