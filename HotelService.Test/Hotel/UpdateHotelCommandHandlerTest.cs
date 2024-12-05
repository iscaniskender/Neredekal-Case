using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Command;
using HotelService.Data.Entity;
using HotelService.Data.Enum;
using HotelService.Data.Repository.Hotel;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace HotelService.Test.Hotel;

public class UpdateHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateHotelCommandHandler _handler;
    private readonly Mock<IDistributedCache> _distributedCacheMock;

    public UpdateHotelCommandHandlerTests()
    {
        _distributedCacheMock = new Mock<IDistributedCache>();
        _hotelRepositoryMock = new Mock<IHotelRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateHotelCommandHandler(_hotelRepositoryMock.Object, _mapperMock.Object,_distributedCacheMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHotelIsUpdatedSuccessfully()
    {
        // Arrange
        var command = new UpdateHotelCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Hotel",
            ContactInfos = [new ContactInfoDto { Content = "test@example.com", Type = ContactType.Email.ToString() }],
            AuthorizedPersons = [new AuthorizedPersonDto { FirstName = "John", LastName = " Doe" }]
        };

        var hotelEntity = new HotelEntity
        {
            Id = command.Id,
            Name = "Updated Hotel",
            ContactInfos = [new ContactInfoEntity { Content = "test@example.com", Type = ContactType.Email }],
            AuthorizedPersons = [new AuthorizedPersonEntity { FirstName = "John", LastName = " Doe" }]
        };

        _mapperMock.Setup(m => m.Map<HotelEntity>(command))
            .Returns(hotelEntity);

        _hotelRepositoryMock.Setup(r => r.UpdateHotelAsync(It.IsAny<HotelEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);

        _mapperMock.Verify(m => m.Map<HotelEntity>(command), Times.Once);
        _hotelRepositoryMock.Verify(r => r.UpdateHotelAsync(It.IsAny<HotelEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenMappingFails()
    {
        // Arrange
        var command = new UpdateHotelCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Hotel"
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
        _hotelRepositoryMock.Verify(r => r.UpdateHotelAsync(It.IsAny<HotelEntity>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
    {
        // Arrange
        var command = new UpdateHotelCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Hotel",
            ContactInfos = [new ContactInfoDto { Content = "test@example.com", Type = ContactType.Email.ToString() }],
            AuthorizedPersons = [new AuthorizedPersonDto { FirstName = "John", LastName = " Doe" }]
        };

        var hotelEntity = new HotelEntity
        {
            Id = command.Id,
            Name = "Updated Hotel",
            ContactInfos = [new ContactInfoEntity { Content = "test@example.com", Type = ContactType.Email }],
            AuthorizedPersons = [new AuthorizedPersonEntity { FirstName = "John", LastName = " Doe" }]
        };

        var exceptionMessage = "Database error occurred";

        _mapperMock.Setup(m => m.Map<HotelEntity>(command))
            .Returns(hotelEntity);

        _hotelRepositoryMock.Setup(r => r.UpdateHotelAsync(It.IsAny<HotelEntity>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);

        _mapperMock.Verify(m => m.Map<HotelEntity>(command), Times.Once);
        _hotelRepositoryMock.Verify(r => r.UpdateHotelAsync(It.IsAny<HotelEntity>()), Times.Once);
    }
}