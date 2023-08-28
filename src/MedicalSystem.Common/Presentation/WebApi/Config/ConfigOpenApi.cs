using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Custom OpenAPI (Swagger) setup
/// </summary>
public static class ConfigOpenApi
{
    /// <summary>
    /// Default OpenAPI security
    /// </summary>
    /// <param name="options">Swagger options</param>
    public static void ConfigDefaultSecurity(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        });
    }

    /// <summary>
    /// Add custom OpenAPI hosts
    /// </summary>
    /// <param name="swagger">Swagger document data</param>
    /// <param name="request">HTTP request</param>
    /// <param name="builder">Web application builder</param>
    public static void AddHosts(OpenApiDocument swagger, HttpRequest request, WebApplicationBuilder builder)
    {
        var configValue = builder.Configuration
            .GetSection("Project:Servers")
            .Get<IList<OpenApiServer>>();

        var servers = new List<OpenApiServer>
        {
            new()
            {
                Url = $"{request.Scheme}://{request.Host.Value}",
                Description = "Local environment",
            }
        };

        if (configValue != null && configValue.Any())
            servers.AddRange(configValue);

        swagger.Servers = servers;
    }
}