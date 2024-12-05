using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ReportService.Application.Dto;
using ReportService.Application.Report.Query;
using ReportService.Data.Entity;
using ReportService.Data.Repository;

namespace ReportService.Test.Report;

public class GetAllReportsQueryHandlerTests
{
    private readonly Mock<IReportRepository> _mockReportRepository = new();
    private readonly Mock<IMapper> _mockMapper = new();
    private readonly Mock<ILogger<GetAllReportsQueryHandler>> _mockLogger = new();

    [Fact]
    public async Task Handle_ReportsExist_ReturnsSuccessResultWithMappedReports()
    {
        // Arrange
        var reportEntities = new List<ReportEntity>
        {
            new() { Id = Guid.NewGuid(), Location = "Location1" },
            new() { Id = Guid.NewGuid(), Location = "Location2" }
        };

        var reportDtos = new ReportDto[]
        {
            new() { Id = reportEntities[0].Id, Location = "Location1" },
            new() { Id = reportEntities[1].Id, Location = "Location2" }
        };

        _mockReportRepository
            .Setup(repo => repo.GetAllReportsAsync(new CancellationToken()))
            .ReturnsAsync(reportEntities);

        _mockMapper
            .Setup(mapper => mapper.Map<ReportDto[]>(reportEntities))
            .Returns(reportDtos);

        var handler = new GetAllReportsQueryHandler(
            _mockReportRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAllReportsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(2, result.Data.Length);
        Assert.Equal(reportDtos, result.Data);
        _mockReportRepository.Verify(repo => repo.GetAllReportsAsync(new CancellationToken()), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ReportDto[]>(reportEntities), Times.Once);
    }

    [Fact]
    public async Task Handle_NoReportsExist_ReturnsSuccessResultWithEmptyArray()
    {
        // Arrange
        var reportEntities = new List<ReportEntity>();
        var reportDtos = Array.Empty<ReportDto>();

        _mockReportRepository
            .Setup(repo => repo.GetAllReportsAsync(new CancellationToken()))
            .ReturnsAsync(reportEntities);

        _mockMapper
            .Setup(mapper => mapper.Map<ReportDto[]>(reportEntities))
            .Returns(reportDtos);

        var handler = new GetAllReportsQueryHandler(
            _mockReportRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAllReportsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Empty(result.Data);
        _mockReportRepository.Verify(repo => repo.GetAllReportsAsync(new CancellationToken()), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<ReportDto[]>(reportEntities), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ReturnsFailureResult()
    {
        // Arrange
        var expectedException = new Exception("Repository Error");

        _mockReportRepository
            .Setup(repo => repo.GetAllReportsAsync(new CancellationToken()))
            .ThrowsAsync(expectedException);

        var handler = new GetAllReportsQueryHandler(
            _mockReportRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAllReportsQuery(), CancellationToken.None);

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
        var reportEntities = new List<ReportEntity>
        {
            new() { Id = Guid.NewGuid(), Location = "Location1" }
        };

        var expectedException = new Exception("Mapping Error");

        _mockReportRepository
            .Setup(repo => repo.GetAllReportsAsync(new CancellationToken()))
            .ReturnsAsync(reportEntities);

        _mockMapper
            .Setup(mapper => mapper.Map<ReportDto[]>(reportEntities))
            .Throws(expectedException);

        var handler = new GetAllReportsQueryHandler(
            _mockReportRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetAllReportsQuery(), CancellationToken.None);

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