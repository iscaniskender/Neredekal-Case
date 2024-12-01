﻿using Microsoft.Extensions.DependencyInjection;

namespace ReportService.Application.MediatR;

public static class MediatRDependencyHandler
{
    public static IServiceCollection RegisterRequestHandlers(
        this IServiceCollection services)
    {
        return services
            .AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(MediatRDependencyHandler).Assembly));
    }
}