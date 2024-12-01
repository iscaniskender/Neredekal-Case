using App.Core.Results;
using HotelService.Data.Repository;
using HotelService.Data.Repository.ContactInfo;
using HotelService.Data.Repository.Hotel;
using MediatR;

namespace HotelService.Application.ContactInfo.Command;

public class DeleteContactInfoCommandHandler : IRequestHandler<DeleteContactInfoCommand, Result<Unit>>
{
    private readonly IContactInfoRepository _contactInfoRepository;

    public DeleteContactInfoCommandHandler(IContactInfoRepository contactInfoRepository)
    {
        _contactInfoRepository = contactInfoRepository;
    }

    public async Task<Result<Unit>> Handle(DeleteContactInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _contactInfoRepository.DeleteContactInfoAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception ex)
        {
            return Result<Unit>.Failure(ex.Message);
        }
    }
}