using AutoMapper;
using HotelService.Application.AuthorizedPerson.Command;
using HotelService.Data.Entity;
using HotelService.Data.Repository.AuthorizedPerson;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelService.Test.AuthorizedPerson;

public class CreateAuthorizedPersonCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuthorizedPersonRepository> _authorizedPersonRepositoryMock;
    private readonly Mock<ILogger<CreateAuthorizedPersonCommandHandler>> _loggerMock;
    private readonly CreateAuthorizedPersonCommandHandler _handler;

    public CreateAuthorizedPersonCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _authorizedPersonRepositoryMock = new Mock<IAuthorizedPersonRepository>();
        _loggerMock = new Mock<ILogger<CreateAuthorizedPersonCommandHandler>>();
        _handler = new CreateAuthorizedPersonCommandHandler(
            _mapperMock.Object,
            _authorizedPersonRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAuthorizedPersonIsCreated()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            // Command properties
            FirstName = "John",
            LastName = "Doe"
        };

        var authorizedPersonEntity = new AuthorizedPersonEntity
        {
            // Entity properties
            FirstName = "John",
            LastName = "Doe"
        };

        _mapperMock.Setup(m => m.Map<AuthorizedPersonEntity>(command))
            .Returns(authorizedPersonEntity);

        _authorizedPersonRepositoryMock.Setup(r => r.AddAuthorizedPersonAsync(It.IsAny<AuthorizedPersonEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);

        _mapperMock.Verify(m => m.Map<AuthorizedPersonEntity>(command), Times.Once);
        _authorizedPersonRepositoryMock.Verify(r => r.AddAuthorizedPersonAsync(It.IsAny<AuthorizedPersonEntity>()), Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            FirstName = "John",
            LastName = "Smith"
        };

        var authorizedPersonEntity = new AuthorizedPersonEntity
        {
            FirstName = "John",
            LastName = "Smith"
        };

        var exceptionMessage = "Database error";

        _mapperMock.Setup(m => m.Map<AuthorizedPersonEntity>(command))
            .Returns(authorizedPersonEntity);

        _authorizedPersonRepositoryMock.Setup(r => r.AddAuthorizedPersonAsync(It.IsAny<AuthorizedPersonEntity>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _mapperMock.Verify(m => m.Map<AuthorizedPersonEntity>(command), Times.Once);
        _authorizedPersonRepositoryMock.Verify(r => r.AddAuthorizedPersonAsync(It.IsAny<AuthorizedPersonEntity>()), Times.Once);

        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenMappingFails()
    {
        // Arrange
        var command = new CreateAuthorizedPersonCommand
        {
            FirstName = "Jane",
            LastName = "Doe"
        };

        var exceptionMessage = "Mapping failed";

        _mapperMock.Setup(m => m.Map<AuthorizedPersonEntity>(command))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _mapperMock.Verify(m => m.Map<AuthorizedPersonEntity>(command), Times.Once);
        _authorizedPersonRepositoryMock.Verify(r => r.AddAuthorizedPersonAsync(It.IsAny<AuthorizedPersonEntity>()), Times.Never);

        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }

}