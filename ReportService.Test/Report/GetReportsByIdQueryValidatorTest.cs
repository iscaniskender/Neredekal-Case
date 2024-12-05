using FluentAssertions;
using ReportService.Application.Report.Query;
using ReportService.Application.Validator;

namespace ReportService.Test.Report
{
    public class GetReportsByIdQueryValidatorTests
    {
        private readonly GetReportsByIdQueryValidator _validator = new();

        [Fact]
        public void Should_Have_Validation_Error_When_Id_Is_Empty()
        {
            // Arrange
            var query = new GetReportsByIdQuery(Guid.Empty);

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == "Id" && error.ErrorMessage == "Report ID cannot be empty.");
        }

        [Fact]
        public void Should_Pass_Validation_When_Id_Is_Valid()
        {
            // Arrange
            var query = new GetReportsByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.Validate(query);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}