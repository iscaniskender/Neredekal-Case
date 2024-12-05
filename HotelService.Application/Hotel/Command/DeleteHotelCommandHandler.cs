using App.Core.Enum;
using App.Core.Results;
using HotelService.Data.Repository.Hotel;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HotelService.Application.Hotel.Command
{
    public class DeleteHotelCommandHandler(IHotelRepository hotelRepository,IDistributedCache distributedCache)
        : IRequestHandler<DeleteHotelCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
               await hotelRepository.DeleteHotelAsync(request.Id);
               await distributedCache.RemoveAsync(Const.HotelListCacheKey, cancellationToken);
               return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception e)
            {
                return Result<Unit>.Failure(e.Message);
            }
        }
    }
}
