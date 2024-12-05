using FluentAssertions;
using ReportService.Application.Report.Command;
using ReportService.Application.Validator;

namespace ReportService.Test.Report
{
    public class CreateReportCommandValidatorTests
    {
        private readonly CreateReportCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Validation_Error_When_Location_Is_Empty()
        {
            // Arrange
            var command = new CreateReportCommand(string.Empty);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == "Location" && error.ErrorMessage == "Location is required.");
        }

        [Fact]
        public void Should_Have_Validation_Error_When_Location_Is_Too_Short()
        {
            // Arrange
            var command = new CreateReportCommand("AB");

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == "Location" && error.ErrorMessage == "Location must be between 3 and 100 characters.");
        }

        [Fact]
        public void Should_Have_Validation_Error_When_Location_Is_Too_Long()
        {
            // Arrange
            var command = new CreateReportCommand(new string('A', 101));

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == "Location" && error.ErrorMessage == "Location must be between 3 and 100 characters.");
        }

        [Fact]
        public void Should_Pass_Validation_When_Location_Is_Valid()
        {
            // Arrange
            var command = new CreateReportCommand("Valid Location");

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
