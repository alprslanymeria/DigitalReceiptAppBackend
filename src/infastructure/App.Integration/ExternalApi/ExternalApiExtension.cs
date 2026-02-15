using App.Application.Contracts.Infrastructure.ExternalApi;
using App.Domain.Options.ExternalAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Integration.ExternalApi;

public static class ExternalApiExtension
{
    public static IServiceCollection AddExternalApiServicesExt(this IServiceCollection services, IConfiguration configuration)
    {

        // USER API CLIENT (HTTPCLIENT FACTORY)
        var userApiOptions = configuration.GetSection(UserApiOptions.Key).Get<UserApiOptions>();

        services.AddHttpClient<IUserApiClient, UserApiClient>(client =>
        {
            if (userApiOptions is not null)
            {
                client.BaseAddress = new Uri(userApiOptions.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(userApiOptions.TimeoutSeconds);
            }
        });

        return services;
    }
}
