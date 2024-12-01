using App.Core.Results;
using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Data.Repository;
using HotelService.Data.Repository.ContactInfo;
using MediatR;

namespace HotelService.Application.ContactInfo.Query;

public class GetContactInfoByHotelIdQueryHandler :IRequestHandler<GetContactInfoByHotelIdQuery,Result<ContactInfoDto[]>>
{
    private readonly IContactInfoRepository _contactInfoRepository;
    private readonly IMapper _mapper;

    public GetContactInfoByHotelIdQueryHandler(IContactInfoRepository contactInfoRepository, IMapper mapper)
    {
        _contactInfoRepository = contactInfoRepository;
        _mapper = mapper;
    }

    public async Task<Result<ContactInfoDto[]>> Handle(GetContactInfoByHotelIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var contact = await _contactInfoRepository.GetContactInfosByHotelIdAsync(request.HotelId);
            return Result<ContactInfoDto[]>.Success(_mapper.Map<ContactInfoDto[]>(contact));
        }
        catch (Exception e)
        {
           return Result<ContactInfoDto[]>.Failure(e.Message);
        }
    }
}