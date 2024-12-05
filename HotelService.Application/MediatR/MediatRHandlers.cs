using Microsoft.Extensions.DependencyInjection;

namespace HotelService.Application.MediatR;

public static class MediatRDependencyHandler
{
    public static IServiceCollection RegisterRequestHandlers(
        this IServiceCollection services)
    {
        return services
            .AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(MediatRDependencyHandler).Assembly));
    }
}