using FluentValidation.TestHelper;
using HotelService.Application.Hotel.Query;
using HotelService.Application.Hotel.Validator;

namespace HotelService.Test.Hotel
{
    public class GetHotelByIdQueryValidatorTest
    {
        private readonly GetHotelByIdQueryValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_Id_IsEmpty()
        {
            // Arrange
            var query = new GetHotelByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Hotel Id cannot be an empty GUID.");
        }

        [Fact]
        public void Should_HaveError_When_Id_IsNotProvided()
        {
            // Arrange
            var query = new GetHotelByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Hotel Id is required.");
        }

        [Fact]
        public void Should_NotHaveError_When_Valid_Id_IsProvided()
        {
            // Arrange
            var query = new GetHotelByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }
    }
}