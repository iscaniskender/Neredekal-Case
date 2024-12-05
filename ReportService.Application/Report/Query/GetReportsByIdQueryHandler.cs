using App.Core.Results;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ReportService.Application.Dto;
using ReportService.Data.Repository;

namespace ReportService.Application.Report.Query;

public class GetReportsByIdQueryHandler(
    IReportRepository reportRepository,
    IMapper mapper,
    ILogger<GetAllReportsQueryHandler> logger)
    : IRequestHandler<GetReportsByIdQuery, Result<ReportDto>>
{
    public async Task<Result<ReportDto>> Handle(GetReportsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var report = await reportRepository.GetReportByIdAsync(request.Id);
            var result = mapper.Map<ReportDto>(report);
            return Result<ReportDto>.Success(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result<ReportDto>.Failure(e.Message);
        }
    }
}