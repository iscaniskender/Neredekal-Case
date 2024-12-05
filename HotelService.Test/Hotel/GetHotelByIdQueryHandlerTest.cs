using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Query;
using HotelService.Data.Entity;
using HotelService.Data.Repository.Hotel;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelService.Test.Hotel
{
    public class GetHotelByIdQueryHandlerTests
    {
        private readonly Mock<IHotelRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetHotelByIdQueryHandler>> _loggerMock;
        private readonly GetHotelByIdQueryHandler _handler;

        public GetHotelByIdQueryHandlerTests()
        {
            _repositoryMock = new Mock<IHotelRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetHotelByIdQueryHandler>>();
            
            _handler = new GetHotelByIdQueryHandler(
                _repositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_WhenHotelExists_ReturnsSuccessResult()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var query = new GetHotelByIdQuery(hotelId);
            var hotel = new HotelEntity() { Id = hotelId, Name = "Test Hotel" };
            var hotelDto = new HotelDto { Id = hotelId, Name = "Test Hotel" };

            _repositoryMock.Setup(x => x.GetHotelByIdAsync(hotelId))
                .ReturnsAsync(hotel);
            
            _mapperMock.Setup(x => x.Map<HotelDto>(hotel))
                .Returns(hotelDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(hotelDto, result.Data);
            _repositoryMock.Verify(x => x.GetHotelByIdAsync(hotelId), Times.Once);
            _mapperMock.Verify(x => x.Map<HotelDto>(hotel), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_ReturnsFailureResult()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var query = new GetHotelByIdQuery(hotelId);
            var exceptionMessage = "Database connection failed";
            
            _repositoryMock.Setup(x => x.GetHotelByIdAsync(hotelId))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(exceptionMessage, result.Message);
            _repositoryMock.Verify(x => x.GetHotelByIdAsync(hotelId), Times.Once);
            _mapperMock.Verify(x => x.Map<HotelDto>(It.IsAny<HotelEntity>()), Times.Never);
            
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenMappingFails_ReturnsFailureResult()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var query = new GetHotelByIdQuery(hotelId);
            var hotel = new HotelEntity() { Id = hotelId, Name = "Test Hotel" };
            var exceptionMessage = "Mapping configuration is invalid";

            _repositoryMock.Setup(x => x.GetHotelByIdAsync(hotelId))
                .ReturnsAsync(hotel);

            _mapperMock.Setup(x => x.Map<HotelDto>(hotel))
                .Throws(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(exceptionMessage, result.Message);
            _repositoryMock.Verify(x => x.GetHotelByIdAsync(hotelId), Times.Once);
            _mapperMock.Verify(x => x.Map<HotelDto>(hotel), Times.Once);
            
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}