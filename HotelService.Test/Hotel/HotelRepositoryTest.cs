using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelService.Data.Context;
using HotelService.Data.Entity;
using HotelService.Data.Enum;
using HotelService.Data.Repository.Hotel;

namespace HotelService.Test.Hotel;

public class HotelRepositoryTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly HotelDbContext _context;
    private readonly HotelRepository _repository;
    private readonly IConfiguration _configuration;

    public HotelRepositoryTests()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var solutionPath = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.FullName;
        var configPath = Path.Combine(solutionPath!, "HotelService.Api", "appsettings.Development.json");

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configPath, optional: false)
            .Build();

        var connectionString = _configuration.GetConnectionString("HotelDatabase");
        
        var services = new ServiceCollection();
        services.AddDbContext<HotelDbContext>(options =>
            options.UseNpgsql(connectionString));

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<HotelDbContext>();
        _context.Database.EnsureCreated();

        _repository = new HotelRepository(_context);
    }

    private async Task SeedTestData()
    {
        var hotels = new List<HotelEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Hotel 1",
                IsActive = true,
                ContactInfos = new List<ContactInfoEntity>
                {
                    new()
                    {
                        Type = ContactType.Location,
                        Content = "Istanbul, Turkey"
                    }
                },
                AuthorizedPersons =
                [
                    new AuthorizedPersonEntity
                    {
                        FirstName = "John Doe",
                        LastName = "john@test.com"
                    }
                ]
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Hotel 2",
                IsActive = true,
                ContactInfos = new List<ContactInfoEntity>
                {
                    new()
                    {
                        Type = ContactType.Location,
                        Content = "Ankara, Turkey"
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Inactive Hotel",
                IsActive = false,
                ContactInfos = new List<ContactInfoEntity>
                {
                    new()
                    {
                        Type = ContactType.Location,
                        Content = "Izmir, Turkey"
                    }
                }
            }
        };

        await _context.Hotels.AddRangeAsync(hotels);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllHotelsAsync_ReturnsOnlyActiveHotels()
    {
        // Arrange
        await SeedTestData();

        // Act
        var hotels = await _repository.GetAllHotelsAsync();

        // Assert
        Assert.NotNull(hotels);
        Assert.All(hotels, h => Assert.True(h.IsActive));
        Assert.All(hotels, h => Assert.NotNull(h.ContactInfos));
        Assert.All(hotels, h => Assert.NotNull(h.AuthorizedPersons));
    }

    [Fact]
    public async Task GetHotelByIdAsync_ExistingActiveHotel_ReturnsHotel()
    {
        await SeedTestData();
        var activeHotel = await _context.Hotels.FirstAsync(h => h.IsActive);

        var hotel = await _repository.GetHotelByIdAsync(activeHotel.Id);

        Assert.NotNull(hotel);
        Assert.Equal(activeHotel.Id, hotel.Id);
        Assert.NotNull(hotel.ContactInfos);
        Assert.NotNull(hotel.AuthorizedPersons);
    }

    [Fact]
    public async Task GetHotelByIdAsync_NonExistentId_ReturnsNull()
    {
        await SeedTestData();
        var nonExistentId = Guid.NewGuid();

        var hotel = await _repository.GetHotelByIdAsync(nonExistentId);

        Assert.Null(hotel);
    }

    [Fact]
    public async Task AddHotelAsync_AddsHotelToDatabase()
    {
        var newHotel = new HotelEntity
        {
            Name = "New Test Hotel",
            IsActive = true,
            ContactInfos = new List<ContactInfoEntity>
            {
                new()
                {
                    Type = ContactType.Location,
                    Content = "Test Location"
                }
            }
        };

        await _repository.AddHotelAsync(newHotel);

        var addedHotel = await _context.Hotels
            .Include(h => h.ContactInfos)
            .FirstOrDefaultAsync(h => h.Name == "New Test Hotel");
            
        Assert.NotNull(addedHotel);
        Assert.Equal("Test Location", addedHotel.ContactInfos.First().Content);
    }

    [Fact]
    public async Task UpdateHotelAsync_UpdatesExistingHotel()
    {
        await SeedTestData();
        var hotelToUpdate = await _context.Hotels.FirstAsync(h => h.IsActive);
        hotelToUpdate.Name = "Updated Hotel Name";

        await _repository.UpdateHotelAsync(hotelToUpdate);

        var updatedHotel = await _context.Hotels.FindAsync(hotelToUpdate.Id);
        Assert.Equal("Updated Hotel Name", updatedHotel.Name);
    }
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _serviceProvider.Dispose();
    }
}