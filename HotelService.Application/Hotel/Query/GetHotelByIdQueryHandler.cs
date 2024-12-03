using MediatR;
using App.Core.Results;
using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Data.Repository.Hotel;
using Microsoft.Extensions.Logging;

namespace HotelService.Application.Hotel.Query
{
    public class GetHotelByIdQueryHandler(
        IHotelRepository repository,
        IMapper mapper,
        ILogger<GetHotelByIdQueryHandler> logger)
        : IRequestHandler<GetHotelByIdQuery, Result<HotelDto>>
    {
        public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await repository.GetHotelByIdAsync(request.Id);
                return Result<HotelDto>.Success(mapper.Map<HotelDto>(hotel));
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<HotelDto>.Failure(e.Message);
            }
        }
    }
}
