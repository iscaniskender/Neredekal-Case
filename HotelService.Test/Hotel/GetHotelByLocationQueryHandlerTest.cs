using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Query;
using HotelService.Data.Entity;
using HotelService.Data.Enum;
using HotelService.Data.Repository.Hotel;
using Moq;

namespace HotelService.Test.Hotel
{
    public class GetHotelByLocationQueryHandlerTests
    {
        private readonly Mock<IHotelRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetHotelByLocationQueryHandler _handler;

        public GetHotelByLocationQueryHandlerTests()
        {
            _repositoryMock = new Mock<IHotelRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetHotelByLocationQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenHotelsExistForLocation_ReturnsSuccessResultWithHotels()
        {
            // Arrange
            var location = "New York";
            var query = new GetHotelByLocationQuery(location);
            
            var hotels = new[]
            {
                new HotelEntity 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Hotel 1",
                    ContactInfos = new List<ContactInfoEntity> 
                    { 
                        new() { Type = ContactType.Location , Content = location }
                    }
                },
                new HotelEntity 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Hotel 2",
                    ContactInfos = new List<ContactInfoEntity> 
                    { 
                        new() { Type = ContactType.Location , Content = location }
                    }
                }
            };

            var hotelDtos = new[]
            {
                new HotelDto 
                { 
                    Id = hotels[0].Id, 
                    Name = "Hotel 1",
                    ContactInfos = new List<ContactInfoDto> 
                    { 
                        new() { Type = ContactType.Location.ToString() , Content = location }
                    }
                },
                new HotelDto 
                { 
                    Id = hotels[1].Id, 
                    Name = "Hotel 2",
                    ContactInfos = new List<ContactInfoDto> 
                    { 
                        new() { Type = ContactType.Location.ToString() , Content = location }
                    }
                }
            };

            _repositoryMock.Setup(x => x.GetHotelsByConditionAsync(location))
                .ReturnsAsync(hotels);
            _mapperMock.Setup(x => x.Map<HotelDto[]>(hotels))
                .Returns(hotelDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(2, result.Data.Length);
            Assert.All(result.Data, hotel => 
                Assert.Contains(hotel.ContactInfos, c => c.Content == location));
        }

        [Fact]
        public async Task Handle_WhenHotelHasMultipleLocations_ReturnsHotelWithMatchingLocation()
        {
            // Arrange
            var targetLocation = "New York";
            var query = new GetHotelByLocationQuery(targetLocation);
            
            var hotel = new HotelEntity 
            { 
                Id = Guid.NewGuid(), 
                Name = "Multi-Location Hotel",
                ContactInfos = new List<ContactInfoEntity> 
                { 
                    new() { Content = "London" },
                    new() { Content = targetLocation },
                    new() { Content = "Tokyo" }
                }
            };

            var hotelDto = new HotelDto 
            { 
                Id = hotel.Id, 
                Name = "Multi-Location Hotel",
                ContactInfos = new List<ContactInfoDto> 
                { 
                    new() { Content = "London" },
                    new() { Content = targetLocation },
                    new() { Content = "Tokyo" }
                }
            };

            _repositoryMock.Setup(x => x.GetHotelsByConditionAsync(targetLocation))
                .ReturnsAsync(new[] { hotel });
            _mapperMock.Setup(x => x.Map<HotelDto[]>(It.IsAny<HotelEntity[]>()))
                .Returns(new[] { hotelDto });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Single(result.Data);
            Assert.Contains(result.Data[0].ContactInfos, c => c.Content == targetLocation);
        }

        [Fact]
        public async Task Handle_WhenNoHotelsExistForLocation_ReturnsSuccessResultWithEmptyArray()
        {
            // Arrange
            var location = "Non-existent Location";
            var query = new GetHotelByLocationQuery(location);
            var emptyHotels = Array.Empty<HotelEntity>();
            var emptyDtos = Array.Empty<HotelDto>();

            _repositoryMock.Setup(x => x.GetHotelsByConditionAsync(location))
                .ReturnsAsync(emptyHotels);
            _mapperMock.Setup(x => x.Map<HotelDto[]>(emptyHotels))
                .Returns(emptyDtos);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_ReturnsFailureResult()
        {
            // Arrange
            var location = "New York";
            var query = new GetHotelByLocationQuery(location);
            var exceptionMessage = "Database connection failed";

            _repositoryMock.Setup(x => x.GetHotelsByConditionAsync(location))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(exceptionMessage, result.Message);
            _repositoryMock.Verify(x => x.GetHotelsByConditionAsync(location), Times.Once);
            _mapperMock.Verify(x => x.Map<HotelDto[]>(It.IsAny<HotelEntity[]>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenHotelHasEmptyContactInfos_HandlesGracefully()
        {
            // Arrange
            var location = "New York";
            var query = new GetHotelByLocationQuery(location);
            
            var hotel = new HotelEntity 
            { 
                Id = Guid.NewGuid(), 
                Name = "Hotel Without Contacts",
                ContactInfos = new List<ContactInfoEntity>()
            };

            var hotelDto = new HotelDto 
            { 
                Id = hotel.Id, 
                Name = "Hotel Without Contacts",
                ContactInfos = new List<ContactInfoDto>()
            };

            _repositoryMock.Setup(x => x.GetHotelsByConditionAsync(location))
                .ReturnsAsync(new[] { hotel });
            _mapperMock.Setup(x => x.Map<HotelDto[]>(It.IsAny<HotelEntity[]>()))
                .Returns(new[] { hotelDto });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Empty(result.Data[0].ContactInfos);
        }
    }
}