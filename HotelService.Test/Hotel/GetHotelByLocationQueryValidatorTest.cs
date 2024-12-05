using FluentValidation.TestHelper;
using HotelService.Application.Hotel.Query;
using HotelService.Application.Hotel.Validator;

namespace HotelService.Test.Hotel
{
    public class GetHotelByLocationQueryValidatorTest
    {
        private readonly GetHotelByLocationQueryValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_Location_IsEmpty()
        {
            // Arrange
            var query = new GetHotelByLocationQuery(string.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Location)
                .WithErrorMessage("Location is required.");
        }

        [Fact]
        public void Should_HaveError_When_Location_IsLessThan_3Characters()
        {
            // Arrange
            var query = new GetHotelByLocationQuery("AB");

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Location)
                .WithErrorMessage("Location must be between 3 and 100 characters.");
        }

        [Fact]
        public void Should_HaveError_When_Location_IsMoreThan_100Characters()
        {
            // Arrange
            var query = new GetHotelByLocationQuery(new string('A', 101));

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Location)
                .WithErrorMessage("Location must be between 3 and 100 characters.");
        }

        [Fact]
        public void Should_NotHaveError_When_Location_IsValid()
        {
            // Arrange
            var query = new GetHotelByLocationQuery("Valid Location");

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Location);
        }
    }
}
