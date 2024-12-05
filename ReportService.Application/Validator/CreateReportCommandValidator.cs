using FluentValidation;
using ReportService.Application.Report.Command;

namespace ReportService.Application.Validator;

public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    public CreateReportCommandValidator()
    {
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .Length(3, 100).WithMessage("Location must be between 3 and 100 characters.");
    }
}