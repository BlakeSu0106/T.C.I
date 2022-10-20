using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Telligent.Consumer.Identity.Application.Swagger;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize =
            context.MethodInfo.DeclaringType != null &&
            (context.MethodInfo.DeclaringType
                 .GetCustomAttributes(true)
                 .OfType<AuthorizeAttribute>()
                 .Any() ||
             context.MethodInfo
                 .GetCustomAttributes(true)
                 .OfType<AuthorizeAttribute>().Any());

        if (!hasAuthorize) return;

        operation.Responses.Add("401", new OpenApiResponse {Description = "Unauthorized"});
        operation.Responses.Add("403", new OpenApiResponse {Description = "Forbidden"});

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                [
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    }
                ] = new List<string>()
            },
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            }
        };
    }
}