using App.Core.Results;
using AutoMapper;
using HotelService.Data.Entity;
using HotelService.Data.Repository;
using HotelService.Data.Repository.AuthorizedPerson;
using HotelService.Data.Repository.Hotel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelService.Application.AuthorizedPerson.Command;

public class CreateAuthorizedPersonCommandHandler(
    IMapper mapper,
    IAuthorizedPersonRepository authorizedPersonRepository,
    ILogger<CreateAuthorizedPersonCommandHandler> logger)
    : IRequestHandler<CreateAuthorizedPersonCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CreateAuthorizedPersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var person = mapper.Map<AuthorizedPersonEntity>(request);
            await authorizedPersonRepository.AddAuthorizedPersonAsync(person);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result<Unit>.Failure(e.Message);
        }
    }
}