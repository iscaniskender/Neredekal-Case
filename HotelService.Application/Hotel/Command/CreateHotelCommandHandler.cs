using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Results;
using AutoMapper;
using HotelService.Data.Entity;
using HotelService.Data.Repository.Hotel;
using Microsoft.Extensions.Logging;

namespace HotelService.Application.Hotel.Command
{
    public class CreateHotelCommandHandler(
        IHotelRepository hotelRepository,
        IMapper mapper,
        ILogger<CreateHotelCommandHandler> logger)
        : IRequestHandler<CreateHotelCommand, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = mapper.Map<HotelEntity>(request);
                await hotelRepository.AddHotelAsync(hotel);
                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return Result<Unit>.Failure(e.Message);
            }
        }
    }
}
