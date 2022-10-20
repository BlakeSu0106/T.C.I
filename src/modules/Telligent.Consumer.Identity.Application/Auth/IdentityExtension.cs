using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Telligent.Consumer.Identity.Database;
using Telligent.Consumer.Identity.Domain.Identities;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Telligent.Consumer.Identity.Application.Auth;

public static class IdentityExtension
{
    private const string AuthSectionKey = "Auth";

    public static IServiceCollection AddAuthServer(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(AuthSectionKey);

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        context.HttpContext.Response.Redirect(
                            $"{section.GetValue<string>("IdentityWebUri")}{new Uri(context.RedirectUri).Query}");

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });

        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<IdentityDbContext>()
                    .ReplaceDefaultEntities<IdentityApplication, IdentityAuthorization, IdentityScope, IdentityToken,
                        Guid>();

                options.UseQuartz();
            })
            .AddServer(options =>
            {
                options
                    .AllowAuthorizationCodeFlow()
                    .RequireProofKeyForCodeExchange()
                    .AllowClientCredentialsFlow()
                    .AllowRefreshTokenFlow()
                    .AllowPasswordFlow();

                options
                    .SetAuthorizationEndpointUris("/connect/authorize")
                    .SetLogoutEndpointUris("/connect/logout")
                    .SetTokenEndpointUris("/connect/token")
                    .SetUserinfoEndpointUris("/connect/userinfo");

                // This method should only be used during development. On production, using a X.509 certificate is recommended.
                // 要確認一下 ephemeralEncryption 跟 Development 的差異
                options
                    .AddEphemeralEncryptionKey()
                    .AddEphemeralSigningKey()
                    .DisableAccessTokenEncryption();

                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, "api");

                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .DisableTransportSecurityRequirement();
            })
            .AddValidation(options =>
            {
                options.UseLocalServer();
                options.UseAspNetCore();
            });

        return services;
    }
}