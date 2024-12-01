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

namespace HotelService.Application.Hotel.Command
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result<Unit>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public CreateHotelCommandHandler(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = _mapper.Map<HotelEntity>(request);
                await _hotelRepository.AddHotelAsync(hotel);
                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception e)
            {
                return Result<Unit>.Failure(e.Message);
            }
        }
    }
}
