using FluentValidation;
using HotelService.Application.Hotel.Query;

namespace HotelService.Application.Hotel.Validator;

public class GetHotelByLocationQueryValidator:AbstractValidator<GetHotelByLocationQuery>
{
    public GetHotelByLocationQueryValidator()
    {
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .Length(3, 100).WithMessage("Location must be between 3 and 100 characters.");
    }
}