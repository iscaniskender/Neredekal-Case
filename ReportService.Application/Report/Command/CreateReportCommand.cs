using App.Core.Results;
using MediatR;

namespace ReportService.Application.Report.Command;

public class CreateReportCommand : IRequest<Result<Unit>>
{
    public string Location { get; set; }

    public CreateReportCommand(string location)
    {
        Location = location;
    }
}