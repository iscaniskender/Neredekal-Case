using MediatR;
using App.Core.Results;

namespace HotelService.Application.Hotel.Command
{
    public class DeleteHotelCommand(Guid id) : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; } = id;
    }
}
