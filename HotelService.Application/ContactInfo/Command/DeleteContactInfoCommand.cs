using App.Core.Results;
using MediatR;

namespace HotelService.Application.ContactInfo.Command;

public class DeleteContactInfoCommand(Guid id) : IRequest<Result<Unit>>
{
    public Guid Id { get; set; } = id;
}