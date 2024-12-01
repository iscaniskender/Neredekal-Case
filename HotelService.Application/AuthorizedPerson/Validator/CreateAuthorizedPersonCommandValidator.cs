using FluentValidation;
using HotelService.Application.AuthorizedPerson.Command;

namespace HotelService.Application.AuthorizedPerson.Validator;

public class CreateAuthorizedPersonCommandValidator : AbstractValidator<CreateAuthorizedPersonCommand>
{
    public CreateAuthorizedPersonCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Hotel ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Hotel ID must be a valid GUID.");
    }
}
