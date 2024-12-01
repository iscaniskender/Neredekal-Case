using App.Core.Results;
using HotelService.Application.Hotel.Dto;
using MediatR;

namespace HotelService.Application.ContactInfo.Query;

public class GetContactInfoByHotelIdQuery :IRequest<Result<ContactInfoDto[]>>
{
    public Guid HotelId { get; set; }

    public GetContactInfoByHotelIdQuery(Guid hotelId)
    {
        HotelId = hotelId;
    }
}