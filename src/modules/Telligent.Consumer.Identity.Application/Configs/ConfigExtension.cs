using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Telligent.Consumer.Identity.Application.Configs;

public static class ConfigExtension
{
    private const string ConfigSectionKey = "configs";

    public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Config>(configuration.GetSection(ConfigSectionKey));
        return services;
    }
}