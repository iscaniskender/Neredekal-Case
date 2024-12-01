using App.Core.Results;
using HotelService.Application.Hotel.Dto;
using MediatR;

namespace HotelService.Application.Hotel.Command
{
    public class UpdateHotelCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ContactInfoDto> ContactInfos { get; set; } = new();
        public List<AuthorizedPersonDto> AuthorizedPersons { get; set; } = new();
    }
}
