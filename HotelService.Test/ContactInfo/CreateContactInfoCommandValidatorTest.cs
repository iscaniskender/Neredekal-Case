using FluentValidation.TestHelper;
using HotelService.Application.ContactInfo.Command;
using HotelService.Application.ContactInfo.Validator;
using HotelService.Data.Enum;

namespace HotelService.Test.ContactInfo;

public class CreateContactInfoCommandValidatorTests
{
    private readonly CreateContactInfoCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_Type_IsInvalid()
    {
        // Arrange
        var command = new CreateContactInfoCommand
        {
            Type = "FAKS", 
            Content = "test@example.com",
            HotelId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Type)
              .WithErrorMessage("Type must be a valid ContactType value.");
    }

    [Fact]
    public void Should_HaveError_When_Content_IsEmpty()
    {
        // Arrange
        var command = new CreateContactInfoCommand
        {
            Type = ContactType.Email.ToString(),
            Content = string.Empty,
            HotelId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Content)
              .WithErrorMessage("Content cannot be empty.");
    }

    [Fact]
    public void Should_HaveError_When_HotelId_IsEmpty()
    {
        // Arrange
        var command = new CreateContactInfoCommand
        {
            Type = ContactType.PhoneNumber.ToString(),
            Content = "123-456-7890",
            HotelId = Guid.Empty
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.HotelId)
              .WithErrorMessage("HotelId cannot be empty.");
    }

    [Fact]
    public void Should_Pass_When_AllFields_AreValid()
    {
        // Arrange
        var command = new CreateContactInfoCommand
        {
            Type = ContactType.Email.ToString(),
            Content = "test@example.com",
            HotelId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
