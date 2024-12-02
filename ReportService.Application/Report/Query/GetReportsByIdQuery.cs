using App.Core.Results;
using MediatR;
using ReportService.Application.Dto;

namespace ReportService.Application.Report.Query;

public class GetReportsByIdQuery : IRequest<Result<ReportDto>>
{
    public Guid Id { get; set; }

    public GetReportsByIdQuery(Guid id)
    {
        Id = id;
    }
}