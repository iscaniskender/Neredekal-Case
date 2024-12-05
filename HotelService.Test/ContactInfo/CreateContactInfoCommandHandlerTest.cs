using AutoMapper;
using HotelService.Application.ContactInfo.Command;
using HotelService.Data.Entity;
using HotelService.Data.Enum;
using HotelService.Data.Repository.ContactInfo;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelService.Test.ContactInfo;

public class CreateContactInfoCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IContactInfoRepository> _contactInfoRepositoryMock;
    private readonly Mock<ILogger<CreateContactInfoCommandHandler>> _loggerMock;
    private readonly CreateContactInfoCommandHandler _handler;

    public CreateContactInfoCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _contactInfoRepositoryMock = new Mock<IContactInfoRepository>();
        _loggerMock = new Mock<ILogger<CreateContactInfoCommandHandler>>();
        _handler = new CreateContactInfoCommandHandler(
            _mapperMock.Object,
            _contactInfoRepositoryMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenContactInfoIsCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateContactInfoCommand
        {
            Content = "test@example.com",
            Type = ContactType.Email.ToString(),
            HotelId = Guid.NewGuid()
        };

        var contactInfoEntity = new ContactInfoEntity
        {
            Content = "test@example.com",
            Type = ContactType.Email,
            HotelId = command.HotelId
        };

        _mapperMock.Setup(m => m.Map<ContactInfoEntity>(command))
            .Returns(contactInfoEntity);

        _contactInfoRepositoryMock.Setup(r => r.AddContactInfoAsync(It.IsAny<ContactInfoEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);

        _mapperMock.Verify(m => m.Map<ContactInfoEntity>(command), Times.Once);
        _contactInfoRepositoryMock.Verify(r => r.AddContactInfoAsync(It.IsAny<ContactInfoEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenMappingFails()
    {
        // Arrange
        var command = new CreateContactInfoCommand
        {
            Content = "test@example.com",
            Type = ContactType.Email.ToString(),
            HotelId = Guid.NewGuid()
        };

        var exceptionMessage = "Mapping failed";

        _mapperMock.Setup(m => m.Map<ContactInfoEntity>(command))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _mapperMock.Verify(m => m.Map<ContactInfoEntity>(command), Times.Once);
        _contactInfoRepositoryMock.Verify(r => r.AddContactInfoAsync(It.IsAny<ContactInfoEntity>()), Times.Never);
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
        var command = new CreateContactInfoCommand
        {
            Content = "test@example.com",
            Type = ContactType.Email.ToString(),
            HotelId = Guid.NewGuid()
        };

        var contactInfoEntity = new ContactInfoEntity
        {
            Content = "test@example.com",
            Type = ContactType.Email,
            HotelId = command.HotelId
        };

        var exceptionMessage = "Database error occurred";

        _mapperMock.Setup(m => m.Map<ContactInfoEntity>(command))
            .Returns(contactInfoEntity);

        _contactInfoRepositoryMock.Setup(r => r.AddContactInfoAsync(It.IsAny<ContactInfoEntity>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _mapperMock.Verify(m => m.Map<ContactInfoEntity>(command), Times.Once);
        _contactInfoRepositoryMock.Verify(r => r.AddContactInfoAsync(It.IsAny<ContactInfoEntity>()), Times.Once);
        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }
}