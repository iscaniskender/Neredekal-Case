using App.Core.Results;
using MediatR;

namespace HotelService.Application.AuthorizedPerson.Command;

public class CreateAuthorizedPersonCommand : IRequest<Result<Unit>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid HotelId { get; set; }
}