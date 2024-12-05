using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Application.Mapping;
using ReportService.Application.MediatR;

namespace ReportService.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services,
        IConfigurationManager configManager)
    {
        services.RegisterRequestHandlers();
        services.AddAutoMapper(typeof(ReportMapping));
        return services;
    }
}