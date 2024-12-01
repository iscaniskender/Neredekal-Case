using App.Core.Results;
using MediatR;

namespace HotelService.Application.AuthorizedPerson.Command;

public class DeleteAuthorizedPersonCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }

    public DeleteAuthorizedPersonCommand(Guid id)
    {
        Id = id;
    }
}