using AutoMapper;
using ReportService.Application.Dto;
using ReportService.Application.Report.Command;
using ReportService.Data.Entity;

namespace ReportService.Application.Mapping;

public class ReportMapping : Profile
{
    public ReportMapping()
    {
        CreateMap<ReportEntity, ReportDto>().ReverseMap();
    }
}