using App.Core.Results;
using App.Core.Shared.Messages;
using MassTransit;
using MediatR;
using ReportService.Data.Entity;
using ReportService.Data.Repository;

namespace ReportService.Application.Report.Command;

public class CreateReportCommandHandler:IRequestHandler<CreateReportCommand,Result<Unit>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IPublishEndpoint _publisherEndpoint;

    public CreateReportCommandHandler(IReportRepository reportRepository,
        IPublishEndpoint publisherEndpoint)
    {
        _reportRepository = reportRepository;
        _publisherEndpoint = publisherEndpoint;
    }

    public async Task<Result<Unit>> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reportId =await _reportRepository.AddReportAsync(new ReportEntity());
        
            await _publisherEndpoint.Publish(new ReportCreatedEvent{ ReportId = reportId, Location = request.Location}, cancellationToken);
        
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            return Result<Unit>.Failure(e.Message);
        }
    }
}