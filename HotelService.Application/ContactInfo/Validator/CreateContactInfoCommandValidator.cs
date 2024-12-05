using FluentValidation;
using HotelService.Application.ContactInfo.Command;
using HotelService.Data.Enum;

namespace HotelService.Application.ContactInfo.Validator;

public class CreateContactInfoCommandValidator : AbstractValidator<CreateContactInfoCommand>
{
    public CreateContactInfoCommandValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type cannot be empty.")
            .Must(BeAValidContactType)
            .WithMessage("Type must be a valid ContactType value.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content cannot be empty.");

        RuleFor(x => x.HotelId)
            .NotEmpty()
            .WithMessage("HotelId cannot be empty.");
    }
    private bool BeAValidContactType(string type)
    {
        return Enum.TryParse(typeof(ContactType), type, true, out _);
    }
}
