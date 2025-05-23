using System;
using System.Reflection;
using System.Text.Json.Serialization;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Message broker services configuration with dependency injection
/// </summary>
public static class ConfigMessageBroker
{
    /// <summary>
    /// Message broker setup (with RabbitMQ)
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <param name="configuration">System configuration</param>
    /// <param name="configureEndpointsAction">Optional action to configure specific receive endpoints.</param> // <--- NUEVO PARÁMETRO
    /// <returns>Service descriptors collection with custom services</returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services,
        IConfiguration configuration,
        Action<IRabbitMqBusFactoryConfigurator, IRegistrationContext>? configureEndpointsAction = null) // <--- NUEVO PARÁMETRO
    {
        string rabbitMqHost = Environment.GetEnvironmentVariable("SYSTEM_MB_HOST");
        string rabbitMqUser = Environment.GetEnvironmentVariable("SYSTEM_MB_USER");
        string rabbitMqPass = Environment.GetEnvironmentVariable("SYSTEM_MB_PASS");

        var settings = configuration.Get<CustomConfig>();
        string projectName = settings?.Project?.Name ?? "EmptyProjectName";

        services.AddMassTransit(busConfigurator =>
        {
            // Add consumers by reflection
            busConfigurator.AddConsumers(Assembly.GetEntryAssembly());

            // MassTransit general setup
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
            {
                // Json serializer setup
                busFactoryConfigurator.ConfigureJsonSerializerOptions(opts =>
                {
                    // Add enumerator converter
                    opts.Converters.Add(new JsonStringEnumConverter());
                    return opts;
                });

                busFactoryConfigurator.Host(rabbitMqHost, hostConfigurator =>
                {
                    hostConfigurator.Username(rabbitMqUser);
                    hostConfigurator.Password(rabbitMqPass);
                });

                // Configuración automática de endpoints
                busFactoryConfigurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(projectName, false));

                // <--- NUEVA LÍNEA: Ejecutar la acción de configuración de endpoints específicos
                configureEndpointsAction?.Invoke(busFactoryConfigurator, context);
            });
        });

        return services;
    }
}