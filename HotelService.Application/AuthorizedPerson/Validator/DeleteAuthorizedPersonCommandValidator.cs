using FluentValidation;

namespace HotelService.Application.AuthorizedPerson.Command
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