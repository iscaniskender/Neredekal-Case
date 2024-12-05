using FluentValidation.TestHelper;
using HotelService.Application.Hotel.Command;
using HotelService.Application.Hotel.Validator;
using Xunit;
using System;

namespace HotelService.Test.Hotel
{
    public class DeleteHotelCommandHandlerValidatorTest
    {
        private readonly DeleteHotelCommandHandlerValidator _validator = new();

        [Fact]
        public void Should_HaveError_When_Id_IsEmpty()
        {
            // Arrange
            var command = new DeleteHotelCommand(Guid.Empty);  // Test boş GUID

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Hotel Id cannot be an empty GUID.");
        }

        [Fact]
        public void Should_NotHaveError_When_Valid_Id_IsProvided()
        {
            // Arrange
            var command = new DeleteHotelCommand(Guid.NewGuid());  // Geçerli GUID

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }
    }
}