using App.Core.Results;
using MediatR;

namespace HotelService.Application.AuthorizedPerson.Command;

public class DeleteAuthorizedPersonCommand(Guid id) : IRequest<Result<Unit>>
{
    public Guid Id { get; set; } = id;
}