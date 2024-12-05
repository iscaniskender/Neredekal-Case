using FluentValidation.TestHelper;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Command;
using HotelService.Application.Hotel.Validator;
namespace HotelService.Test.Hotel
{
    public class CreateHotelCommandValidatorTests
    {
        private readonly CreateHotelCommandValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_Name_IsEmpty()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                Name = string.Empty
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Hotel name is required.");
        }

        [Fact]
        public void Should_HaveError_When_Name_IsTooLong()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                Name = new string('A', 201) // 201 karakter uzunluğunda bir isim
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Hotel name must not exceed 200 characters.");
        }

        [Fact]
        public void Should_HaveError_When_ContactInfos_IsEmpty()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                ContactInfos = new List<ContactInfoDto>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ContactInfos)
                  .WithErrorMessage("At least one contact info is required.");
        }

        [Fact]
        public void Should_HaveError_When_InvalidContactInfo()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                ContactInfos = [new ContactInfoDto { Type = null, Content = "" }]
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ContactInfos)
                  .WithErrorMessage("Invalid contact info.");
        }

        [Fact]
        public void Should_HaveError_When_AuthorizedPersons_IsEmpty()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                AuthorizedPersons = new List<AuthorizedPersonDto>()
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AuthorizedPersons)
                  .WithErrorMessage("At least one authorized person is required.");
        }

        [Fact]
        public void Should_HaveError_When_InvalidAuthorizedPerson()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                AuthorizedPersons = [new() { FirstName = "", LastName = "" }]
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.AuthorizedPersons)
                  .WithErrorMessage("Invalid authorized person.");
        }

        [Fact]
        public void Should_NotHaveError_When_ValidHotelCommand()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                Name = "Test Hotel",
                ContactInfos = [new ContactInfoDto { Type = "Email", Content = "test@example.com" }],
                AuthorizedPersons = [new AuthorizedPersonDto { FirstName = "John", LastName = "Doe" }]
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.ContactInfos);
            result.ShouldNotHaveValidationErrorFor(x => x.AuthorizedPersons);
        }
    }
}
