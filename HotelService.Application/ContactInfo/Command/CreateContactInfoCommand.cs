using System.Net.Mime;
using App.Core.Results;
using MediatR;

namespace HotelService.Application.ContactInfo.Command;

public class CreateContactInfoCommand : IRequest<Result<Unit>>
{
    public string Type { get; set; }
    public string Content { get; set; }
    public Guid HotelId { get; set; }
}