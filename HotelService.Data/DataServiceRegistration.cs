using HotelService.Data.Context;
using HotelService.Data.Repository;
using HotelService.Data.Repository.AuthorizedPerson;
using HotelService.Data.Repository.ContactInfo;
using HotelService.Data.Repository.Hotel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Data.Context;

namespace HotelService.Data;

public static class DataServiceRegistration
{
    public static IServiceCollection ConfigureData(this IServiceCollection services,
        IConfigurationManager configManager)
    {
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IAuthorizedPersonRepository, AuthorizedPersonRepository>();
        services.AddScoped<IContactInfoRepository, ContactInfoRepository>();

        services.AddDbContext<HotelDbContext>(options =>
            options.UseNpgsql(configManager.GetConnectionString("HotelDatabase")));

        return services;
    }

    public static void ApplyMigrations(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
        dbContext.Database.Migrate();
    }
}