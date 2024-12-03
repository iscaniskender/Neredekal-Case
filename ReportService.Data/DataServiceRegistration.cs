using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Data.Context;
using ReportService.Data.Repository;

namespace ReportService.Data;

public static class DataServiceRegistration
{
    public static IServiceCollection ConfigureData(this IServiceCollection services,
        IConfigurationManager configManager)
    {
        services.AddScoped<IReportRepository, ReportRepository>();

        services.AddDbContext<ReportDbContext>(options =>
            options.UseNpgsql(configManager.GetConnectionString("ReportDatabase")));
        
        return services;
    }
    
    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
        dbContext.Database.Migrate();
    }
}