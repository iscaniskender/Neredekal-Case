using FluentValidation;
using HotelService.Application.Hotel.Query;

namespace HotelService.Application.Hotel.Validator;

public class GetHotelByIdQueryValidator:AbstractValidator<GetHotelByIdQuery>
{
    public GetHotelByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Hotel Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Hotel Id cannot be an empty GUID.");
    }
}