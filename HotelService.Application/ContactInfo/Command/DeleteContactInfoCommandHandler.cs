using App.Core.Results;
using HotelService.Data.Repository;
using HotelService.Data.Repository.ContactInfo;
using HotelService.Data.Repository.Hotel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HotelService.Application.ContactInfo.Command;

public class DeleteContactInfoCommandHandler(
    IContactInfoRepository contactInfoRepository,
    ILogger<DeleteContactInfoCommandHandler> logger)
    : IRequestHandler<DeleteContactInfoCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteContactInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await contactInfoRepository.DeleteContactInfoAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result<Unit>.Failure(e.Message);
        }
    }
}