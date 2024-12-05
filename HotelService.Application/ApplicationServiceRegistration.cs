using HotelService.Application.Mapping;
using HotelService.Application.MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelService.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterRequestHandlers();
        services.AddAutoMapper(typeof(HotelMapping));
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Redis:Connection").Value;
            options.InstanceName = "RedisInstance";
        });

        return services;
    }
}