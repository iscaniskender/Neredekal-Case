using App.Core.Results;
using AutoMapper;
using MediatR;
using ReportService.Application.Dto;
using ReportService.Data.Repository;

namespace ReportService.Application.Report.Query;

public class GetReportsByIdQueryHandler :IRequestHandler<GetReportsByIdQuery, Result<ReportDto>>
{
    private readonly IReportRepository _reportRepository;
    private readonly IMapper _mapper;

    public GetReportsByIdQueryHandler(IReportRepository reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    public async Task<Result<ReportDto>> Handle(GetReportsByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _reportRepository.GetReportByIdAsync(request.Id);
            var result = _mapper.Map<ReportDto>(report);
            return Result<ReportDto>.Success(result);
        }
        catch (Exception e)
        {
            return Result<ReportDto>.Failure(e.Message);
        }
    }
}