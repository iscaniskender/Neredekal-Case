using App.Core.Results;
using MediatR;
using ReportService.Application.Dto;

namespace ReportService.Application.Report.Query;

public class GetAllReportsQuery :IRequest<Result<ReportDto[]>> {}
