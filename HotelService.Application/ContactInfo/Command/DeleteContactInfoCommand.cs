using App.Core.Results;
using MediatR;

namespace HotelService.Application.ContactInfo;

public class DeleteContactInfoCommand : IRequest<Result<Unit>>
{
    public Guid Id { get; set; }

    public DeleteContactInfoCommand(Guid id)
    {
        Id = id;
    }
}