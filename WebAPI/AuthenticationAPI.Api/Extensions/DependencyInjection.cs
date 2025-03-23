using AuthenticationAPI.Api.Application.Identity;
using AuthenticationAPI.Api.Application.Identity.Keycloak;
using Microsoft.Extensions.Options;

namespace AuthenticationAPI.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddKeycloakAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeycloakOptions>(configuration.GetSection("KeycloakOptions"));

        services.AddTransient<IIdentityProvider, IdentityProvider>();
        services.AddTransient<KeycloakService>();
        services.AddTransient<KeycloakHttpMessageHandler>();

        services.AddHttpClient<KeycloakService>((serviceProvider, config) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

            config.BaseAddress = new Uri(keycloakOptions.AdminUrl);

        })
        .AddHttpMessageHandler<KeycloakHttpMessageHandler>();

        return services;
    }
}
