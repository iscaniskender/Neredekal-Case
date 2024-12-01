using App.Core.Results;
using AutoMapper;
using HotelService.Data.Entity;
using HotelService.Data.Repository;
using HotelService.Data.Repository.AuthorizedPerson;
using HotelService.Data.Repository.Hotel;
using MediatR;

namespace HotelService.Application.AuthorizedPerson.Command;

public class CreateAuthorizedPersonCommandHandler : IRequestHandler<CreateAuthorizedPersonCommand, Result<Unit>>
{
    private readonly IAuthorizedPersonRepository _authorizedPersonRepository;
    private readonly IMapper _mapper;

    public CreateAuthorizedPersonCommandHandler(IMapper mapper, IAuthorizedPersonRepository authorizedPersonRepository)
    {
        _mapper = mapper;
        _authorizedPersonRepository = authorizedPersonRepository;
    }

    public async Task<Result<Unit>> Handle(CreateAuthorizedPersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var person = _mapper.Map<AuthorizedPersonEntity>(request);
            await _authorizedPersonRepository.AddAuthorizedPersonAsync(person);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            return Result<Unit>.Failure(e.Message);
        }
    }
}