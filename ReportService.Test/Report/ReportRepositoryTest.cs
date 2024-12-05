using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Data.Context;
using ReportService.Data.Entity;
using ReportService.Data.Enum;
using ReportService.Data.Repository;


namespace ReportService.Test.Report;

public class ReportRepositoryTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ReportDbContext _context;
    private readonly ReportRepository _repository;
    private readonly IConfiguration _configuration;

    public ReportRepositoryTests()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        
        var solutionPath = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.FullName;
        
        var configPath = Path.Combine(solutionPath!, "ReportService.Api", "appsettings.Development.json");

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configPath, optional: false)
            .Build();

        var connectionString = _configuration.GetConnectionString("ReportDatabase");
        
        var services = new ServiceCollection();
        
        services.AddDbContext<ReportDbContext>(options =>
            options.UseNpgsql(connectionString));

        _serviceProvider = services.BuildServiceProvider();
        
        _context = _serviceProvider.GetRequiredService<ReportDbContext>();
        _context.Database.EnsureCreated();

        _repository = new ReportRepository(_context);
    }

    private async Task SeedTestData()
    {
        var reports = new List<ReportEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Location = "Location1",
                IsActive = true,
                Status = ReportStatus.Preparing
            },
            new()
            {
                Id = Guid.NewGuid(),
                Location = "Location2",
                IsActive = true,
                Status = ReportStatus.Preparing
            },
            new()
            {
                Id = Guid.NewGuid(),
                Location = "Location3",
                IsActive = false,
                Status = ReportStatus.Completed
            }
        };

        await _context.Reports.AddRangeAsync(reports);
        await _context.SaveChangesAsync();
    }


    [Fact]
    public async Task GetAllReportsAsync_ReturnsOnlyActiveReports()
    {
        // Arrange
        await SeedTestData();

        // Act
        var reports = await _repository.GetAllReportsAsync();

        // Assert
        Assert.NotNull(reports); 
        Assert.All(reports, r => Assert.True(r.IsActive)); // Tüm raporlar aktif olmalı
    }


    [Fact]
    public async Task GetReportByIdAsync_ExistingActiveReport_ReturnsReport()
    {
        await SeedTestData();
        var activeReport = _context.Reports.First(r => r.IsActive);

        var report = await _repository.GetReportByIdAsync(activeReport.Id);

        Assert.NotNull(report);
        Assert.Equal(activeReport.Id, report.Id);
    }

    [Fact]
    public async Task GetReportByIdAsync_NonExistentId_ReturnsNull()
    {
        await SeedTestData();
        var nonExistentId = Guid.NewGuid();

        var report = await _repository.GetReportByIdAsync(nonExistentId);

        Assert.Null(report);
    }

    [Fact]
    public async Task AddReportAsync_AddsReportToDatabase()
    {
        var newReport = new ReportEntity
        {
            Location = "Test Location",
            IsActive = true,
            Status = ReportStatus.Preparing
        };

        var reportId = await _repository.AddReportAsync(newReport);

        var addedReport = await _context.Reports.FindAsync(reportId);
        Assert.NotNull(addedReport);
        Assert.Equal("Test Location", addedReport.Location);
    }

    [Fact]
    public async Task UpdateReportAsync_UpdatesExistingReport()
    {
        await SeedTestData();
        var reportToUpdate = _context.Reports.First(r => r.IsActive);
        reportToUpdate.Location = "Updated Location";

        await _repository.UpdateReportAsync(reportToUpdate);

        var updatedReport = await _context.Reports.FindAsync(reportToUpdate.Id);
        Assert.Equal("Updated Location", updatedReport.Location);
    }

    [Fact]
    public async Task GetReportsByStatusAsync_ReturnsReportsWithMatchingStatus()
    {
        await SeedTestData();

        var preparingReports = await _repository.GetReportsByStatusAsync(ReportStatus.Preparing);
        var completedReports = await _repository.GetReportsByStatusAsync(ReportStatus.Completed);
        
        Assert.All(preparingReports, r => Assert.Equal(ReportStatus.Preparing, r.Status));
        Assert.All(completedReports, r => Assert.Equal(ReportStatus.Completed, r.Status));
    }

    public void Dispose()
    {
        _context.Dispose();
        _serviceProvider.Dispose();
    }
}