using App.Core.Results;
using HotelService.Data.Repository.AuthorizedPerson;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelService.Application.AuthorizedPerson.Command;

public class DeleteAuthorizedPersonCommandHandler(IAuthorizedPersonRepository authorizedPersonRepository,
    ILogger<DeleteAuthorizedPersonCommandHandler> logger)
    : IRequestHandler<DeleteAuthorizedPersonCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteAuthorizedPersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await authorizedPersonRepository.DeleteAuthorizedPersonAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result<Unit>.Failure(e.Message);
        }
    }
}