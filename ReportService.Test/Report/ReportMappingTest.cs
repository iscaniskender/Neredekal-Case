using System.Globalization;
using AutoMapper;
using FluentAssertions;
using ReportService.Application.Dto;
using ReportService.Application.Mapping;
using ReportService.Data.Entity;

namespace ReportService.Test.Report
{
    public class ReportMappingTests
    {
        private readonly IMapper _mapper;

        public ReportMappingTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ReportMapping>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Should_Map_ReportEntity_To_ReportDto_Correctly()
        {
            // Arrange
            var reportEntity = new ReportEntity
            {
                Id = Guid.NewGuid(),
                Location = "New York",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var reportDto = _mapper.Map<ReportDto>(reportEntity);

            // Assert
            reportDto.Should().NotBeNull();
            reportDto.Id.Should().Be(reportEntity.Id);
            reportDto.Location.Should().Be(reportEntity.Location);
            reportDto.CreatedAt.Should().Be(reportEntity.CreatedAt.ToString());
        }

        [Fact]
        public void Should_Map_ReportDto_To_ReportEntity_Correctly()
        {
            // Arrange
            var reportDto = new ReportDto
            {
                Id = Guid.NewGuid(),
                Location = "London",
                CreatedAt = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            };

            // Act
            var reportEntity = _mapper.Map<ReportEntity>(reportDto);

            // Assert
            reportEntity.Should().NotBeNull();
            reportEntity.Id.Should().Be(reportDto.Id);
            reportEntity.Location.Should().Be(reportDto.Location);
            reportEntity.CreatedAt.Should().Be(Convert.ToDateTime(reportDto.CreatedAt));
        }
    }
}