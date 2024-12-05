using FluentValidation.TestHelper;
using HotelService.Application.AuthorizedPerson.Command;
using HotelService.Application.AuthorizedPerson.Validator;

namespace HotelService.Test.AuthorizedPerson;

public class DeleteAuthorizedPersonCommandValidatorTests
{
    private readonly DeleteAuthorizedPersonCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_Id_IsEmpty()
    {
        // Arrange
        var command = new DeleteAuthorizedPersonCommand(Guid.Empty);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id must be a valid GUID.");
    }

    [Fact]
    public void Should_HaveError_When_Id_IsNotProvided()
    {
        // Arrange
        var command = new DeleteAuthorizedPersonCommand(default);

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Pass_When_Id_IsValid()
    {
        // Arrange
        var command = new DeleteAuthorizedPersonCommand(Guid.NewGuid());
        

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}