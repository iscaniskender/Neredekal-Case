using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReportService.Client.Hotel;
using ReportService.Client.Model;

namespace ReportService.Client;

public static class ClientServiceRegistration
{
    public static IServiceCollection ConfigureClient(this IServiceCollection services,
        IConfigurationManager configManager)
    {
        services.AddScoped<IHotelServiceClient, HotelServiceClient>();
        
        services.Configure<ServiceUrls>(configManager.GetSection("ServiceUrls"));
        
        services.AddHttpClient("HotelServiceClient", (sp, client) =>
        {
            var config = sp.GetRequiredService<IOptions<ServiceUrls>>().Value;
            client.BaseAddress = new Uri(config.HotelServiceUrl);
        });

        return services;
    }
}