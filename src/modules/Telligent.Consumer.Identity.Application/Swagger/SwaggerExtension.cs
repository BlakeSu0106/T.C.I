using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Telligent.Consumer.Identity.Application.Swagger;

public static class SwaggerExtension
{
    /// <summary>
    /// integrate swagger support
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenUri = new Uri($"{configuration.GetSection("Auth").GetValue<string>("Authority")}/connect/token",
            UriKind.Absolute);

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo {Title = "Telexpress Telligent API", Version = "v1.0.0"});

            options.EnableAnnotations();

            // add client credentials support
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = tokenUri
                    }
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = tokenUri
                    }
                }
            });

            options.AddSecurityDefinition("Token", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Name = "Authorization"
            });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
            options.OperationFilter<RequiredHeaderParameterFilter>();
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Telexpress Telligent API v1.0.0");
            options.OAuthClientId("telligent");
            options.OAuthClientSecret("telligent@mecpd");
            options.OAuthAppName("telligent API");
            options.OAuthUsePkce();
        });

        return app;
    }
}