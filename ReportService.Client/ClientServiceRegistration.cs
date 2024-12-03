using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Client.Hotel;

namespace ReportService.Client;

public static class ClientServiceRegistration
{
    public static IServiceCollection ConfigureClient(this IServiceCollection services,
        IConfigurationManager configManager)
    {
        services.AddScoped<IHotelServiceClient, HotelServiceClient>();

        services.AddHttpClient("HotelServiceClient", client =>
        {
            client.BaseAddress = new Uri("http://hotelservice-api:8080");
        });

        return services;
    }
}