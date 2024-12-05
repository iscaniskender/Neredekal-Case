using App.Core.Enum;
using App.Core.Results;
using MediatR;
using AutoMapper;
using HotelService.Data.Entity;
using HotelService.Data.Repository.Hotel;
using Microsoft.Extensions.Caching.Distributed;

namespace HotelService.Application.Hotel.Command
{
    public class UpdateHotelCommandHandler(IHotelRepository hotelRepository, IMapper mapper,IDistributedCache distributedCache)
        : IRequestHandler<UpdateHotelCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = mapper.Map<HotelEntity>(request);
                await hotelRepository.UpdateHotelAsync(hotel);
                await distributedCache.RemoveAsync(Const.HotelListCacheKey, cancellationToken);
                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Failure(ex.Message);
            }
        }
    }
}
