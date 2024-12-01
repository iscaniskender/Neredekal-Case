using HotelService.Data.Repository.Hotel;
using MediatR;
using App.Core.Results;
using AutoMapper;
using HotelService.Application.Dto;

namespace HotelService.Application.Hotel.Query
{
    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, Result<HotelDto[]>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;
        
        public GetAllHotelsQueryHandler(IHotelRepository hotelrepository,
            IMapper mapper)
        {
            _hotelRepository = hotelrepository;
            _mapper = mapper;
        }

        public async Task<Result<HotelDto[]>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotels = await _hotelRepository.GetAllHotelsAsync();
                var data = _mapper.Map<HotelDto[]>(hotels);

                return Result<HotelDto[]>.Success(data);
            }
            catch (Exception e)
            {
                return Result<HotelDto[]>.Failure(e.Message);
            }
        }
    }
}
