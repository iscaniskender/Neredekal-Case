using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ReportService.Application.Dto;
using ReportService.Application.Report.Query;
using ReportService.Data.Entity;
using ReportService.Data.Repository;

namespace ReportService.Test.Report;

public class GetReportsByIdQueryHandlerTests
{
    private readonly Mock<IReportRepository> _mockReportRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<GetAllReportsQueryHandler>> _mockLogger;

    public GetReportsByIdQueryHandlerTests()
    {
        _mockReportRepository = new Mock<IReportRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<GetAllReportsQueryHandler>>();
    }

    [Fact]
    public async Task Handle_ExistingReport_ReturnsSuccessResultWithMappedReport()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var reportEntity = new ReportEntity
        {
            Id = reportId,
            Location = "Test Location"
        };

        var reportDto = new ReportDto
        {
            Id = reportId,
            Location = "Test Location"
        };

        _mockReportRepository
            .Setup(repo => repo.GetReportByIdAsync(reportId))
            .ReturnsAsync(reportEntity);

        _mockMapper
            .Setup(mapper => mapper.Map<ReportDto>(reportEntity))
            .Returns(reportDto);

        var handler = new GetReportsByIdQueryHandler(
            _mockReportRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        var query = new GetReportsByIdQuery (reportId );

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(reportDto, result.Data);
        Assert.Equal("Test Location", result.Data.Location);
        _mockReportRepository.Verify(repo => repo.GetReportByIdAsync(reportId), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ReportDto>(reportEntity), Times.Once);
    }
    
    [Fact]
    public async Task Handle_RepositoryThrowsException_ReturnsFailureResult()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var expectedException = new Exception("Repository Error");

        _mockReportRepository
            .Setup(repo => repo.GetReportByIdAsync(reportId))
            .ThrowsAsync(expectedException);

        var handler = new GetReportsByIdQueryHandler(
            _mockReportRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        var query = new GetReportsByIdQuery (reportId );

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Repository Error", result.Message);
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Repository Error"),
                expectedException,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_MapperThrowsException_ReturnsFailureResult()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var reportEntity = new ReportEntity
        {
            Id = reportId,
            Location = "Test Location"
        };
        var expectedException = new Exception("Mapping Error");

        _mockReportRepository
            .Setup(repo => repo.GetReportByIdAsync(reportId))
            .ReturnsAsync(reportEntity);

        _mockMapper
            .Setup(mapper => mapper.Map<ReportDto>(reportEntity))
            .Throws(expectedException);

        var handler = new GetReportsByIdQueryHandler(
            _mockReportRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        var query = new GetReportsByIdQuery(reportId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Mapping Error", result.Message);
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Mapping Error"),
                expectedException,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}