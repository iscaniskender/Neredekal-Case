using FluentValidation;

namespace HotelService.Application.ContactInfo.Validator
{
    public class DeleteContactInfoCommandValidator : AbstractValidator<DeleteContactInfoCommand>
    {
        public DeleteContactInfoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");
        }
    }
}