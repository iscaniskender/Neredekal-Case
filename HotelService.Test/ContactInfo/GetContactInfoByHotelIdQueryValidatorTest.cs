using FluentValidation.TestHelper;
using HotelService.Application.ContactInfo.Query;
using HotelService.Application.ContactInfo.Validator;
using Xunit;

namespace HotelService.Test.ContactInfo
{
    public class GetContactInfoByHotelIdQueryValidatorTests
    {
        private readonly GetContactInfoByHotelIdQueryValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_HotelId_IsEmpty()
        {
            // Arrange
            var query = new GetContactInfoByHotelIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.HotelId)
                .WithErrorMessage("Id is required.");
        }

        [Fact]
        public void Should_HaveError_When_HotelId_IsDefaultGuid()
        {
            // Arrange
            var query = new GetContactInfoByHotelIdQuery(default);
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.HotelId)
                .WithErrorMessage("Id must be a valid GUID.");
        }

        [Fact]
        public void Should_NotHaveError_When_HotelId_IsValid()
        {
            // Arrange
            var query = new GetContactInfoByHotelIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.HotelId);
        }
    }
}