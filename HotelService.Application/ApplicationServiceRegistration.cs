using HotelService.Application.Mapping;
using HotelService.Application.MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelService.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services,
        IConfigurationManager configManager)
    {
        services.RegisterRequestHandlers();
        services.AddAutoMapper(typeof(HotelMapping));
        return services;
    }
}