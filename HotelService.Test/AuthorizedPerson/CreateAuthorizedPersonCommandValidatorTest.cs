using FluentValidation.TestHelper;
using HotelService.Application.AuthorizedPerson.Command;
using HotelService.Application.AuthorizedPerson.Validator;

namespace HotelService.Test.AuthorizedPerson;

public class CreateAuthorizedPersonCommandValidatorTests
{
    private readonly CreateAuthorizedPersonCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_FirstName_IsEmpty()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            FirstName = string.Empty,
            LastName = "ValidLastName",
            HotelId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
              .WithErrorMessage("First name is required.");
    }

    [Fact]
    public void Should_HaveError_When_LastName_IsEmpty()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            FirstName = "ValidFirstName",
            LastName = string.Empty,
            HotelId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName)
              .WithErrorMessage("Last name is required.");
    }

    [Fact]
    public void Should_HaveError_When_HotelId_IsEmpty()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            FirstName = "ValidFirstName",
            LastName = "ValidLastName",
            HotelId = Guid.Empty
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.HotelId)
              .WithErrorMessage("Hotel ID must be a valid GUID.");
    }

    [Fact]
    public void Should_Pass_When_AllFields_AreValid()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            FirstName = "ValidFirstName",
            LastName = "ValidLastName",
            HotelId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveError_When_FirstName_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            FirstName = new string('A', 51),
            LastName = "ValidLastName",
            HotelId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName)
              .WithErrorMessage("First name must not exceed 50 characters.");
    }

    [Fact]
    public void Should_HaveError_When_LastName_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            FirstName = "ValidFirstName",
            LastName = new string('B', 51),
            HotelId = Guid.NewGuid()
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName)
              .WithErrorMessage("Last name must not exceed 50 characters.");
    }
}
