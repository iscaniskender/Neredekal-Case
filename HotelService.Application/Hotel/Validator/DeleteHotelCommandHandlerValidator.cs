using FluentValidation;
using HotelService.Application.Hotel.Command;

namespace HotelService.Application.Hotel.Validator;

public class DeleteHotelCommandHandlerValidator:AbstractValidator<DeleteHotelCommand>
{
    public DeleteHotelCommandHandlerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Hotel Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Hotel Id cannot be an empty GUID.");
    }
}