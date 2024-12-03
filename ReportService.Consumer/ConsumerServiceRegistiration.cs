using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Consumer.Configuration;
using ReportService.Consumer.ReportConsumer;

namespace ReportService.Consumer;

public static class ConsumerServiceRegistration
{
    public static IServiceCollection ConfigureMessageConsumer(this IServiceCollection services,
        IConfigurationManager configManager)
    {
        return services.AddMassTransitConfiguration(configManager);
    }

    private static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services,
        IConfigurationManager configManager)
    {
        MassTransitConfiguration massTransitConfiguration = new MassTransitConfiguration();
        configManager.GetSection("MassTransitConfiguration").Bind(massTransitConfiguration);

        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer(typeof(ReportCreatedConsumer));
            
            cfg.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(massTransitConfiguration.Host, massTransitConfiguration.VirtualHost, h =>
                {
                    h.Username(massTransitConfiguration.Username);
                    h.Password(massTransitConfiguration.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}