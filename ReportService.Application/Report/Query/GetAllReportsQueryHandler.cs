using App.Core.Results;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ReportService.Application.Dto;
using ReportService.Data.Repository;

namespace ReportService.Application.Report.Query;

public class GetAllReportsQueryHandler(
    IReportRepository reportRepository,
    IMapper mapper,
    ILogger<GetAllReportsQueryHandler> logger)
    : IRequestHandler<GetAllReportsQuery, Result<ReportDto[]>>
{
    public async Task<Result<ReportDto[]>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reports = await reportRepository.GetAllReportsAsync();
            var result = mapper.Map<ReportDto[]>(reports);
            return Result<ReportDto[]>.Success(result);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return Result<ReportDto[]>.Failure(e.Message);
        }
    }
}