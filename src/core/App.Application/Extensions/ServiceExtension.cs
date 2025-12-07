using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // FLUENT VALIDATION
        services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);

        return services;
    }
}