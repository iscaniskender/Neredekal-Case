using App.Core.Enum;
using App.Core.Helper;
using AutoMapper;
using HotelService.Application.Dto;
using HotelService.Application.Hotel.Query;
using HotelService.Data.Entity;
using HotelService.Data.Repository.Hotel;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace HotelService.Test.Hotel;

public class GetAllHotelsQueryHandlerTests
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IDistributedCache> _distributedCacheMock;
    private readonly GetAllHotelsQueryHandler _handler;

    public GetAllHotelsQueryHandlerTests()
    {
        _hotelRepositoryMock = new Mock<IHotelRepository>();
        _mapperMock = new Mock<IMapper>();
        _distributedCacheMock = new Mock<IDistributedCache>();
        
        _handler = new GetAllHotelsQueryHandler(
            _hotelRepositoryMock.Object,
            _mapperMock.Object,
            _distributedCacheMock.Object
        );
    }

    private static List<HotelEntity> GetSampleHotels() => new()
    {
        new HotelEntity { Id = Guid.NewGuid(), Name = "Hotel One" },
        new HotelEntity { Id = Guid.NewGuid(), Name = "Hotel Two" }
    };

    private static List<HotelDto> GetSampleHotelDtos(List<HotelEntity> hotels) =>
        hotels.Select(h => new HotelDto { Id = h.Id, Name = h.Name }).ToList();

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenHotelsAreFetchedFromCache()
    {
        // Arrange
        var hotelDtos = GetSampleHotelDtos(GetSampleHotels());
        var cachedData = hotelDtos.ToByteArray(); // ByteArrayHelper kullanılabilir

        _distributedCacheMock.Setup(c => c.GetAsync(Const.HotelListCacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedData);

        // Act
        var result = await _handler.Handle(new GetAllHotelsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        Assert.Equal(hotelDtos.Count, result.Data.Length);

        _distributedCacheMock.Verify(c => c.GetAsync(Const.HotelListCacheKey, It.IsAny<CancellationToken>()), Times.Once);
        _hotelRepositoryMock.Verify(r => r.GetAllHotelsAsync(), Times.Never); // Cache kullanıldığı için repository çağrılmaz
    }

    [Fact]
    public async Task Handle_ShouldFetchFromRepositoryAndUpdateCache_WhenCacheIsEmpty()
    {
        var hotels = GetSampleHotels();
        var hotelDtos = GetSampleHotelDtos(hotels);

        _distributedCacheMock.Setup(c => c.GetAsync(Const.HotelListCacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[])null); // Cache boş

        _hotelRepositoryMock.Setup(r => r.GetAllHotelsAsync())
            .ReturnsAsync(hotels.ToArray());

        _mapperMock.Setup(m => m.Map<HotelDto[]>(hotels))
            .Returns(hotelDtos.ToArray());

        _distributedCacheMock.Setup(c => c.SetAsync(Const.HotelListCacheKey, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(new GetAllHotelsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        Assert.Equal(hotelDtos.Count, result.Data.Length);

        _distributedCacheMock.Verify(c => c.GetAsync(Const.HotelListCacheKey, It.IsAny<CancellationToken>()), Times.Once);
        _hotelRepositoryMock.Verify(r => r.GetAllHotelsAsync(), Times.Once); // Cache boş olduğu için repository çağrılır
        _distributedCacheMock.Verify(c => c.SetAsync(Const.HotelListCacheKey, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
