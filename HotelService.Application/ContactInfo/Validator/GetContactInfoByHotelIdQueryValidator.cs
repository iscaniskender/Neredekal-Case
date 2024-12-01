using FluentValidation;
using HotelService.Application.ContactInfo.Query;

namespace HotelService.Application.ContactInfo.Validator;

public class GetContactInfoByHotelIdQueryValidator:AbstractValidator<GetContactInfoByHotelIdQuery>
{
    public GetContactInfoByHotelIdQueryValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");
    }
}