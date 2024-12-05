using App.Core.Results;
using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Data.Repository.ContactInfo;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelService.Application.ContactInfo.Query;

public class GetContactInfoByHotelIdQueryHandler(IContactInfoRepository contactInfoRepository, IMapper mapper,
    ILogger<GetContactInfoByHotelIdQueryHandler> logger)
    : IRequestHandler<GetContactInfoByHotelIdQuery, Result<ContactInfoDto[]>>
{
    public async Task<Result<ContactInfoDto[]>> Handle(GetContactInfoByHotelIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var contact = await contactInfoRepository.GetContactInfosByHotelIdAsync(request.HotelId);
            return Result<ContactInfoDto[]>.Success(mapper.Map<ContactInfoDto[]>(contact));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
           return Result<ContactInfoDto[]>.Failure(e.Message);
        }
    }
}