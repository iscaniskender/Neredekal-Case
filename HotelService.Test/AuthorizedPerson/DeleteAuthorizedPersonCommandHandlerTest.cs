using HotelService.Application.AuthorizedPerson.Command;
using HotelService.Data.Repository.AuthorizedPerson;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelService.Test.AuthorizedPerson;

public class DeleteAuthorizedPersonCommandHandlerTests
{
    private readonly Mock<IAuthorizedPersonRepository> _authorizedPersonRepositoryMock;
    private readonly Mock<ILogger<DeleteAuthorizedPersonCommandHandler>> _loggerMock;
    private readonly DeleteAuthorizedPersonCommandHandler _handler;

    public DeleteAuthorizedPersonCommandHandlerTests()
    {
        _authorizedPersonRepositoryMock = new Mock<IAuthorizedPersonRepository>();
        _loggerMock = new Mock<ILogger<DeleteAuthorizedPersonCommandHandler>>();
        _handler = new DeleteAuthorizedPersonCommandHandler(
            _authorizedPersonRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAuthorizedPersonIsDeletedSuccessfully()
    {
        // Arrange
        var command = new DeleteAuthorizedPersonCommand(new Guid());

        _authorizedPersonRepositoryMock.Setup(r => r.DeleteAuthorizedPersonAsync(command.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);

        _authorizedPersonRepositoryMock.Verify(r => r.DeleteAuthorizedPersonAsync(command.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAuthorizedPersonDoesNotExist()
    {
        // Arrange
        var command = new DeleteAuthorizedPersonCommand(new Guid());

        var exceptionMessage = "Authorized person not found.";

        _authorizedPersonRepositoryMock.Setup(r => r.DeleteAuthorizedPersonAsync(command.Id))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _authorizedPersonRepositoryMock.Verify(r => r.DeleteAuthorizedPersonAsync(command.Id), Times.Once);
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
        var command = new DeleteAuthorizedPersonCommand(new Guid());

        var exceptionMessage = "Database error occurred.";

        _authorizedPersonRepositoryMock.Setup(r => r.DeleteAuthorizedPersonAsync(command.Id))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _authorizedPersonRepositoryMock.Verify(r => r.DeleteAuthorizedPersonAsync(command.Id), Times.Once);
        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }
}