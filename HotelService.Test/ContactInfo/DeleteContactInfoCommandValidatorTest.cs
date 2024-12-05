using FluentValidation.TestHelper;
using HotelService.Application.ContactInfo.Command;
using HotelService.Application.ContactInfo.Validator;

namespace HotelService.Test.ContactInfo
{
    public class DeleteContactInfoCommandValidatorTests
    {
        private readonly DeleteContactInfoCommandValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_Id_IsEmpty()
        {
            // Arrange
            var command = new DeleteContactInfoCommand(Guid.Empty);
            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id is required.");
        }

        [Fact]
        public void Should_HaveError_When_Id_IsDefaultGuid()
        {
            // Arrange
            var command = new DeleteContactInfoCommand(default);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Id must be a valid GUID.");
        }

        [Fact]
        public void Should_NotHaveError_When_Id_IsValid()
        {
            // Arrange
            var command = new DeleteContactInfoCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }
    }
}