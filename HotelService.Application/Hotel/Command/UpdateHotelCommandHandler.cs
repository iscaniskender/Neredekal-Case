using App.Core.Results;
using MediatR;
using AutoMapper;
using HotelService.Data.Entity;
using HotelService.Data.Repository.Hotel;

namespace HotelService.Application.Hotel.Command
{
    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, Result<Unit>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public UpdateHotelCommandHandler(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = _mapper.Map<HotelEntity>(request);
                await _hotelRepository.UpdateHotelAsync(hotel);
                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Failure(ex.Message);
            }
        }
    }
}
