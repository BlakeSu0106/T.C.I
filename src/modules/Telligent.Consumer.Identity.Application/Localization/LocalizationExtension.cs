using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Telligent.Consumer.Identity.Application.Localization;

public static class LocalizationExtension
{
    public static IServiceCollection AddSelfLocalization(this IServiceCollection services)
    {
        services.AddMvcCore().AddDataAnnotationsLocalization(options =>
        {
            options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(LocalizeResource));
        });

        services.AddLocalization(options => options.ResourcesPath = "");

        return services;
    }

    public static IApplicationBuilder UseSelfLocalization(this IApplicationBuilder app)
    {
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("zh-TW"),
            SupportedCultures = new List<CultureInfo>
            {
                new("zh-CN"),
                new("en-US")
            }
        });

        return app;
    }
}