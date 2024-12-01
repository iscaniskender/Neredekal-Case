using App.Core.Results;
using HotelService.Data.Repository;
using HotelService.Data.Repository.AuthorizedPerson;
using HotelService.Data.Repository.Hotel;
using MediatR;

namespace HotelService.Application.AuthorizedPerson.Command;

public class DeleteAuthorizedPersonCommandHandler:IRequestHandler<DeleteAuthorizedPersonCommand,Result<Unit>>
{
    private readonly IAuthorizedPersonRepository _authorizedPersonRepository;

    public DeleteAuthorizedPersonCommandHandler(IAuthorizedPersonRepository authorizedPersonRepository)
    {
        _authorizedPersonRepository = authorizedPersonRepository;
    }

    public async Task<Result<Unit>> Handle(DeleteAuthorizedPersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authorizedPersonRepository.DeleteAuthorizedPersonAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            return Result<Unit>.Failure(e.Message);
        }
    }
}