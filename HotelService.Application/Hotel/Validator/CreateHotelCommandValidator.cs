using FluentValidation;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Command;

namespace HotelService.Application.Hotel.Validator;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Hotel name is required.")
            .MaximumLength(200).WithMessage("Hotel name must not exceed 200 characters.");
            
        RuleFor(x => x.ContactInfos)
            .NotEmpty().WithMessage("At least one contact info is required.")
            .Must(x => x.All(contact => IsValidContactInfo(contact))).WithMessage("Invalid contact info.");

        RuleFor(x => x.AuthorizedPersons)
            .NotEmpty().WithMessage("At least one authorized person is required.")
            .Must(x => x.All(person => IsValidAuthorizedPerson(person))).WithMessage("Invalid authorized person.");
    }

    private static bool IsValidContactInfo(ContactInfoDto contactInfo)
    {
        return !string.IsNullOrEmpty(contactInfo.Type) || !string.IsNullOrEmpty(contactInfo.Content);
    }

    private static bool IsValidAuthorizedPerson(AuthorizedPersonDto person)
    {
        return !string.IsNullOrEmpty(person.FirstName) && !string.IsNullOrEmpty(person.LastName);
    }
}