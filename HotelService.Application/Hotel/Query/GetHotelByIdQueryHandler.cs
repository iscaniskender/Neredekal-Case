using MediatR;
using App.Core.Results;
using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Data.Repository.Hotel;

namespace HotelService.Application.Hotel.Query
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery,Result<HotelDto>>
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;

        public GetHotelByIdQueryHandler(IHotelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<HotelDto>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hotel = await _repository.GetHotelByIdAsync(request.Id);
                return Result<HotelDto>.Success(_mapper.Map<HotelDto>(hotel));
            }
            catch (Exception e)
            {
                return Result<HotelDto>.Failure(e.Message);
            }
        }
    }
}
