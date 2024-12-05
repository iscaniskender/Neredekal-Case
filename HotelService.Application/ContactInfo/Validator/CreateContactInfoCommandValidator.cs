using FluentValidation;
using HotelService.Application.ContactInfo.Command;

namespace HotelService.Application.ContactInfo.Validator;

public class CreateContactInfoCommandValidator : AbstractValidator<CreateContactInfoCommand>
{
    public CreateContactInfoCommandValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be a valid ContactType value.");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content cannot be empty.");

        RuleFor(x => x.HotelId)
            .NotEmpty()
            .WithMessage("HotelId cannot be empty.");
    }
}
