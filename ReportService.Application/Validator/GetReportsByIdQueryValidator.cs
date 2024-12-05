using FluentValidation;
using ReportService.Application.Report.Query;

namespace ReportService.Application.Validator;

public class GetReportsByIdQueryValidator:AbstractValidator<GetReportsByIdQuery>
{
    public GetReportsByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Report ID cannot be empty.");
    }
}