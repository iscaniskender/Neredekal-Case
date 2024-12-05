using App.Core.Results;
using App.Core.Shared.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using ReportService.Data.Entity;
using ReportService.Data.Repository;

namespace ReportService.Application.Report.Command;

public class CreateReportCommandHandler(
    IReportRepository reportRepository,
    IPublishEndpoint publisherEndpoint,
    ILogger<CreateReportCommandHandler> logger)
    : IRequestHandler<CreateReportCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Location))
                return Result<Unit>.Failure("Location is required");
            
            var reportId = await reportRepository.AddReportAsync(new ReportEntity{Location = request.Location});

            var data =new ReportCreatedEvent { ReportId = reportId, Location = request.Location };
            
            await publisherEndpoint.Publish(data, cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result<Unit>.Failure(e.Message);
        }
    }
}