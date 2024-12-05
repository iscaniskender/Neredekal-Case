using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Command;
using HotelService.Data.Entity;
using HotelService.Data.Enum;
using HotelService.Data.Repository.Hotel;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelService.Test.Hotel;

public class CreateHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<CreateHotelCommandHandler>> _loggerMock;
    private readonly CreateHotelCommandHandler _handler;

    public CreateHotelCommandHandlerTests()
    {
        _hotelRepositoryMock = new Mock<IHotelRepository>();
        Mock<IDistributedCache> distributedCacheMock = new();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateHotelCommandHandler>>();
        _handler = new CreateHotelCommandHandler(
            _hotelRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            distributedCacheMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHotelIsCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateHotelCommand
        {
            Name = "Hotel Test"
        };

        var hotelEntity = new HotelEntity
        {
            Name = "Hotel Test"
        };

        _mapperMock.Setup(m => m.Map<HotelEntity>(command))
            .Returns(hotelEntity);

        _hotelRepositoryMock.Setup(r => r.AddHotelAsync(It.IsAny<HotelEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);

        _mapperMock.Verify(m => m.Map<HotelEntity>(command), Times.Once);
        _hotelRepositoryMock.Verify(r => r.AddHotelAsync(It.IsAny<HotelEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenMappingFails()
    {
        // Arrange
        var command = new CreateHotelCommand
        {
            Name = "Hotel Test"
        };

        var exceptionMessage = "Mapping failed";

        _mapperMock.Setup(m => m.Map<HotelEntity>(command))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _mapperMock.Verify(m => m.Map<HotelEntity>(command), Times.Once);
        _hotelRepositoryMock.Verify(r => r.AddHotelAsync(It.IsAny<HotelEntity>()), Times.Never);
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
        var command = new CreateHotelCommand
        {
            Name = "Hotel Test"
        };

        var hotelEntity = new HotelEntity
        {
            Name = "Hotel Test"
        };

        var exceptionMessage = "Database error occurred";

        _mapperMock.Setup(m => m.Map<HotelEntity>(command))
            .Returns(hotelEntity);

        _hotelRepositoryMock.Setup(r => r.AddHotelAsync(It.IsAny<HotelEntity>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _mapperMock.Verify(m => m.Map<HotelEntity>(command), Times.Once);
        _hotelRepositoryMock.Verify(r => r.AddHotelAsync(It.IsAny<HotelEntity>()), Times.Once);
        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRequiredFieldsAreMissing()
    {
        // Arrange
        var command = new CreateHotelCommand
        {
            Name = null // Zorunlu alan eksik
        };

        var exceptionMessage = "Hotel name is required.";

        _mapperMock.Setup(m => m.Map<HotelEntity>(command))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _mapperMock.Verify(m => m.Map<HotelEntity>(command), Times.Once);
        _hotelRepositoryMock.Verify(r => r.AddHotelAsync(It.IsAny<HotelEntity>()), Times.Never);
        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenContactInfosAndAuthorizedPersonsAreProvided()
    {
        // Arrange
        var command = new CreateHotelCommand
        {
            Name = "Hotel Test",
            ContactInfos = new List<ContactInfoDto>
            {
                new ContactInfoDto { Content = "test@example.com", Type = ContactType.Email.ToString() }
            },
            AuthorizedPersons = new List<AuthorizedPersonDto>
            {
                new AuthorizedPersonDto { FirstName = "John", LastName =" Doe"}
            }
        };

        var hotelEntity = new HotelEntity
        {
            Name = "Hotel Test",
            ContactInfos = new List<ContactInfoEntity>
            {
                new ContactInfoEntity { Content = "test@example.com", Type = ContactType.Email }
            },
            AuthorizedPersons = new List<AuthorizedPersonEntity>
            {
                new AuthorizedPersonEntity { FirstName = "John", LastName =" Doe"}
            }
        };

        _mapperMock.Setup(m => m.Map<HotelEntity>(command))
            .Returns(hotelEntity);

        _hotelRepositoryMock.Setup(r => r.AddHotelAsync(It.IsAny<HotelEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);

        _mapperMock.Verify(m => m.Map<HotelEntity>(command), Times.Once);
        _hotelRepositoryMock.Verify(r => r.AddHotelAsync(It.IsAny<HotelEntity>()), Times.Once);
    }

}