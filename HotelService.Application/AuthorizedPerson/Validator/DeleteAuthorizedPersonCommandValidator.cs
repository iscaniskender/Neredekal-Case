using FluentValidation;
using HotelService.Application.AuthorizedPerson.Command;

namespace HotelService.Application.AuthorizedPerson.Validator
{
    public class DeleteAuthorizedPersonCommandValidator : AbstractValidator<DeleteAuthorizedPersonCommand>
    {
        public DeleteAuthorizedPersonCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");
        }
    }
}