using App.Core.Results;
using AutoMapper;
using HotelService.Data.Entity;
using HotelService.Data.Repository;
using HotelService.Data.Repository.ContactInfo;
using HotelService.Data.Repository.Hotel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelService.Application.ContactInfo.Command;

public class CreateContactInfoCommandHandler(IMapper mapper, IContactInfoRepository contactInfoRepository,
    ILogger<CreateContactInfoCommandHandler> logger)
    : IRequestHandler<CreateContactInfoCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CreateContactInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var hotel = mapper.Map<ContactInfoEntity>(request);
            await contactInfoRepository.AddContactInfoAsync(hotel);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result<Unit>.Failure(e.Message);
        }
    }
}