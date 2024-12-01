using App.Core.Results;
using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Data.Repository.Hotel;
using MediatR;

namespace HotelService.Application.Hotel.Query;

public class GetHotelByLocationQueryHandler:IRequestHandler<GetHotelByLocationQuery,Result<HotelDto[]>>
{
    private readonly IHotelRepository repository;
    private readonly IMapper mapper;

    public GetHotelByLocationQueryHandler(IHotelRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<Result<HotelDto[]>> Handle(GetHotelByLocationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data = await repository.GetHotelsByConditionAsync(request.Location);
            return Result<HotelDto[]>.Success(mapper.Map<HotelDto[]>(data));
        }
        catch (Exception e)
        {
            return Result<HotelDto[]>.Failure(e.Message);
        }
    }
}