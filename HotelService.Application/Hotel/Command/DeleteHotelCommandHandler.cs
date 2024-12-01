using App.Core.Results;
using AutoMapper;
using HotelService.Data.Repository.Hotel;
using MediatR;

namespace HotelService.Application.Hotel.Command
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, Result<Unit>>
    {
        private readonly IHotelRepository _hotelRepository;

        public DeleteHotelCommandHandler(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<Result<Unit>> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            try
            {
               await _hotelRepository.DeleteHotelAsync(request.Id);
               return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception e)
            {
                return Result<Unit>.Failure(e.Message);
            }
        }
    }
}
