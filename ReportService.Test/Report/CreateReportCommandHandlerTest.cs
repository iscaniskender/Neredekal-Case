using App.Core.Shared.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using ReportService.Application.Report.Command;
using ReportService.Data.Entity;
using ReportService.Data.Repository;

namespace ReportService.Test.Report;

public class CreateReportCommandHandlerTests
{
    private readonly Mock<IReportRepository> _mockReportRepository;
    private readonly Mock<IPublishEndpoint> _mockPublisherEndpoint;
    private readonly Mock<ILogger<CreateReportCommandHandler>> _mockLogger;

    public CreateReportCommandHandlerTests()
    {
        _mockReportRepository = new Mock<IReportRepository>();
        _mockPublisherEndpoint = new Mock<IPublishEndpoint>();
        _mockLogger = new Mock<ILogger<CreateReportCommandHandler>>();
    }

    [Fact]
    public async Task Handle_ValidReport_ReturnsSuccessResult()
    {
        // Arrange
        var expectedReportId = Guid.NewGuid();
        var command = new CreateReportCommand ("Test Location" );
        
        _mockReportRepository
            .Setup(repo => repo.AddReportAsync(It.IsAny<ReportEntity>()))
            .ReturnsAsync(expectedReportId);

        _mockPublisherEndpoint
            .Setup(publisher => publisher.Publish(It.IsAny<ReportCreatedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateReportCommandHandler(
            _mockReportRepository.Object, 
            _mockPublisherEndpoint.Object, 
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);
        _mockReportRepository.Verify(repo => repo.AddReportAsync(It.Is<ReportEntity>(r => r.Location == "Test Location")), Times.Once);
        _mockPublisherEndpoint.Verify(publisher => publisher.Publish(
            It.Is<ReportCreatedEvent>(e => e.Location == "Test Location" && e.ReportId == expectedReportId), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ReturnsFailureResult()
    {
        // Arrange
        var command = new CreateReportCommand ("Test Location" );
        var expectedException = new Exception("Repository Error");
        
        _mockReportRepository
            .Setup(repo => repo.AddReportAsync(It.IsAny<ReportEntity>()))
            .ThrowsAsync(expectedException);

        var handler = new CreateReportCommandHandler(
            _mockReportRepository.Object, 
            _mockPublisherEndpoint.Object, 
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

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
    public async Task Handle_PublisherThrowsException_ReturnsFailureResult()
    {
        // Arrange
        var expectedReportId = Guid.NewGuid();
        var command = new CreateReportCommand ("Test Location" );
        var expectedException = new Exception("Publisher Error");
        
        _mockReportRepository
            .Setup(repo => repo.AddReportAsync(It.IsAny<ReportEntity>()))
            .ReturnsAsync(expectedReportId);

        _mockPublisherEndpoint
            .Setup(publisher => publisher.Publish(It.IsAny<ReportCreatedEvent>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var handler = new CreateReportCommandHandler(
            _mockReportRepository.Object, 
            _mockPublisherEndpoint.Object, 
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Publisher Error", result.Message);
        _mockLogger.Verify(
            logger => logger.Log(
                LogLevel.Error, 
                It.IsAny<EventId>(), 
                It.Is<It.IsAnyType>((v, t) => v.ToString() == "Publisher Error"), 
                expectedException, 
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
            Times.Once);
    }

    [Fact]
    public async Task Handle_NullLocation_ReturnsFailureResult()
    {
        // Arrange
        var command = new CreateReportCommand  (null );

        var handler = new CreateReportCommandHandler(
            _mockReportRepository.Object, 
            _mockPublisherEndpoint.Object, 
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Location is required", result.Message);
        _mockReportRepository.Verify(repo => repo.AddReportAsync(It.IsAny<ReportEntity>()), Times.Never);
        _mockPublisherEndpoint.Verify(publisher => publisher.Publish(It.IsAny<ReportCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_EmptyLocation_ReturnsFailureResult()
    {
        // Arrange
        var command = new CreateReportCommand (string.Empty) ;

        var handler = new CreateReportCommandHandler(
            _mockReportRepository.Object, 
            _mockPublisherEndpoint.Object, 
            _mockLogger.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Location is required", result.Message);
        _mockReportRepository.Verify(repo => repo.AddReportAsync(It.IsAny<ReportEntity>()), Times.Never);
        _mockPublisherEndpoint.Verify(publisher => publisher.Publish(It.IsAny<ReportCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}