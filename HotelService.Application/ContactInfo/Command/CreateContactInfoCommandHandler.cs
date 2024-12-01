using App.Core.Results;
using AutoMapper;
using HotelService.Data.Entity;
using HotelService.Data.Repository;
using HotelService.Data.Repository.ContactInfo;
using HotelService.Data.Repository.Hotel;
using MediatR;

namespace HotelService.Application.ContactInfo.Command;

public class CreateContactInfoCommandHandler :IRequestHandler<CreateContactInfoCommand, Result<Unit>>
{
    private readonly IContactInfoRepository _contactInfoRepository;
    private readonly IMapper _mapper;

    public CreateContactInfoCommandHandler(IMapper mapper, IContactInfoRepository contactInfoRepository)
    {
        _mapper = mapper;
        _contactInfoRepository = contactInfoRepository;
    }

    public async Task<Result<Unit>> Handle(CreateContactInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var hotel = _mapper.Map<ContactInfoEntity>(request);
            await _contactInfoRepository.AddContactInfoAsync(hotel);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.Message);
        }
    }
}