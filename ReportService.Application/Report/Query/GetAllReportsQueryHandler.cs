using App.Core.Results;
using AutoMapper;
using MediatR;
using ReportService.Application.Dto;
using ReportService.Data.Repository;

namespace ReportService.Application.Report.Query;

public class GetAllReportsQueryHandler :IRequestHandler<GetAllReportsQuery, Result<ReportDto[]>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;

    public GetAllReportsQueryHandler(IReportRepository reportRepository,
        IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReportDto[]>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var reports = await _reportRepository.GetAllReportsAsync();
            var result = _mapper.Map<ReportDto[]>(reports);
            return Result<ReportDto[]>.Success(result);
        }
        catch (Exception e)
        {
            return Result<ReportDto[]>.Failure(e.Message);
        }
    }
}