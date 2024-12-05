using HotelService.Application.ContactInfo;
using HotelService.Application.ContactInfo.Command;
using HotelService.Data.Repository.ContactInfo;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelService.Test.ContactInfo;

public class DeleteContactInfoCommandHandlerTests
{
    private readonly Mock<IContactInfoRepository> _contactInfoRepositoryMock;
    private readonly Mock<ILogger<DeleteContactInfoCommandHandler>> _loggerMock;
    private readonly DeleteContactInfoCommandHandler _handler;

    public DeleteContactInfoCommandHandlerTests()
    {
        _contactInfoRepositoryMock = new Mock<IContactInfoRepository>();
        _loggerMock = new Mock<ILogger<DeleteContactInfoCommandHandler>>();
        _handler = new DeleteContactInfoCommandHandler(
            _contactInfoRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenContactInfoIsDeletedSuccessfully()
    {
        // Arrange
        var command = new DeleteContactInfoCommand(new Guid());

        _contactInfoRepositoryMock.Setup(r => r.DeleteContactInfoAsync(command.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);

        _contactInfoRepositoryMock.Verify(r => r.DeleteContactInfoAsync(command.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenContactInfoDoesNotExist()
    {
        // Arrange
        var command = new DeleteContactInfoCommand(new Guid());

        var exceptionMessage = "Contact info not found.";

        _contactInfoRepositoryMock.Setup(r => r.DeleteContactInfoAsync(command.Id))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _contactInfoRepositoryMock.Verify(r => r.DeleteContactInfoAsync(command.Id), Times.Once);
        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDatabaseThrowsException()
    {
        // Arrange
        var command = new DeleteContactInfoCommand(new Guid());

        var exceptionMessage = "Database error occurred.";

        _contactInfoRepositoryMock.Setup(r => r.DeleteContactInfoAsync(command.Id))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _contactInfoRepositoryMock.Verify(r => r.DeleteContactInfoAsync(command.Id), Times.Once);
        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }
}