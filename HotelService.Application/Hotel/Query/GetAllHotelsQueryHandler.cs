using App.Core.Enum;
using App.Core.Helper;
using HotelService.Data.Repository.Hotel;
using MediatR;
using App.Core.Results;
using AutoMapper;
using HotelService.Application.Dto;
using Microsoft.Extensions.Caching.Distributed;

namespace HotelService.Application.Hotel.Query;

public class GetAllHotelsQueryHandler(
    IHotelRepository hotelRepository,
    IMapper mapper,
    IDistributedCache distributedCache) : IRequestHandler<GetAllHotelsQuery, Result<HotelDto[]>>
{
    public async Task<Result<HotelDto[]>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cachedData = await distributedCache.GetAsync(Const.HotelListCacheKey, cancellationToken);
            if (cachedData != null)
            {
                var cachedHotels = cachedData.FromByteArray<HotelDto[]>();
                return Result<HotelDto[]>.Success(cachedHotels);
            }
            
            var hotels = await hotelRepository.GetAllHotelsAsync();
            var data = mapper.Map<HotelDto[]>(hotels);
            
            await distributedCache.SetAsync(
                Const.HotelListCacheKey,
                data.ToByteArray(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                },
                cancellationToken
            );

            return Result<HotelDto[]>.Success(data);
        }
        catch (Exception e)
        {
            return Result<HotelDto[]>.Failure(e.Message);
        }
    }
}