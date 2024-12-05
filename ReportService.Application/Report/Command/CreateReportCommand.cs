using App.Core.Results;
using MediatR;

namespace ReportService.Application.Report.Command;

public class CreateReportCommand(string location) : IRequest<Result<Unit>>
{
    public string Location { get; set; } = location;
}