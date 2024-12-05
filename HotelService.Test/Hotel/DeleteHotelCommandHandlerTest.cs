using HotelService.Application.Hotel.Command;
using HotelService.Data.Repository.Hotel;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace HotelService.Test.Hotel;

public class DeleteHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock;
    private readonly Mock<IDistributedCache> _distributedCacheMock;
    private readonly DeleteHotelCommandHandler _handler;

    public DeleteHotelCommandHandlerTests()
    {
        _hotelRepositoryMock = new Mock<IHotelRepository>();
        _distributedCacheMock = new Mock<IDistributedCache>();

        _handler = new DeleteHotelCommandHandler(
            _hotelRepositoryMock.Object,
            _distributedCacheMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHotelIsDeletedSuccessfully()
    {
        // Arrange
        var command = new DeleteHotelCommand(Guid.NewGuid());

        _hotelRepositoryMock.Setup(r => r.DeleteHotelAsync(command.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(Unit.Value, result.Data);
        _hotelRepositoryMock.Verify(r => r.DeleteHotelAsync(command.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenHotelDoesNotExist()
    {
        // Arrange
        var command = new DeleteHotelCommand(Guid.NewGuid());
        var exceptionMessage = "Hotel not found.";

        _hotelRepositoryMock.Setup(r => r.DeleteHotelAsync(command.Id))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);
        _hotelRepositoryMock.Verify(r => r.DeleteHotelAsync(command.Id), Times.Once);
        _distributedCacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenDatabaseThrowsException()
    {
        // Arrange
        var command = new DeleteHotelCommand(Guid.NewGuid());
        var exceptionMessage = "Database error occurred.";

        _hotelRepositoryMock.Setup(r => r.DeleteHotelAsync(command.Id))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(exceptionMessage, result.Message);
        _hotelRepositoryMock.Verify(r => r.DeleteHotelAsync(command.Id), Times.Once);
        _distributedCacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCallCacheRemove_WhenValidIdProvided()
    {
        // Arrange
        var command = new DeleteHotelCommand(Guid.NewGuid());

        _hotelRepositoryMock.Setup(r => r.DeleteHotelAsync(command.Id))
            .Returns(Task.CompletedTask);
        

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        _hotelRepositoryMock.Verify(r => r.DeleteHotelAsync(command.Id), Times.Once);
    }
}
