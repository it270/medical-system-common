using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Custom OpenAPI (Swagger) setup
/// </summary>
public static class ConfigOpenApi
{
    /// <summary>
    /// Custom Swagger generator setup
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <param name="enableSecurity">Enable security setup</param>
    /// <returns>Service descriptors collection with custom configuration</returns>
    public static IServiceCollection AddSwaggerGenCustom(this IServiceCollection services,
        bool enableSecurity = true)
    {
        services.AddSwaggerGen(options =>
        {
            // Custom security configuration
            if (enableSecurity)
                options.ConfigDefaultSecurity();

            // Enable Swagger filters
            options.ExampleFilters();

            try
            {
                // Add XML comments
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            }
            catch (Exception)
            {
                Log.Error("XML comments for Swagger disabled");
            }
        });

        // Enable automatic examples search in project
        services.AddSwaggerExamples();

        return services;
    }

    /// <summary>
    /// Custom Swagger setup
    /// </summary>
    /// <param name="app">WebApplication instance</param>
    /// <returns>WebApplication instance with custom configuration</returns>
    public static WebApplication UseSwaggerCustom(this WebApplication app)
    {
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                AddHosts(swagger, httpReq, app);
            });
        });

        return app;
    }

    /// <summary>
    /// Default OpenAPI security
    /// </summary>
    /// <param name="options">Swagger options</param>
    private static void ConfigDefaultSecurity(this SwaggerGenOptions options)
    {
        const string securityDefinitionName = "Bearer";

        options.AddSecurityDefinition(securityDefinitionName, new OpenApiSecurityScheme
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
                        Id = securityDefinitionName
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
    /// <param name="app">Web application</param>
    private static void AddHosts(OpenApiDocument swagger, HttpRequest request, WebApplication app)
    {
        var configValue = app.Configuration
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