// EN It270.MedicalSystem.Common/Presentation/WebApi/Config/ConfigMessageBroker.cs
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
    /// Configura el Message Broker con opciones básicas y auto-descubrimiento de consumidores
    /// en el ensamblado de entrada de la aplicación.
    /// </summary>
    /// <param name="services">Service descriptors collection.</param>
    /// <param name="configuration">System configuration.</param>
    /// <returns>Service descriptors collection con servicios personalizados.</returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMassTransit(busConfigurator =>
        {
            // SetKebabCaseEndpointNameFormatter debe ir aquí, antes de UsingRabbitMq
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, factoryConfigurator) => // Aquí obtenemos el 'context' de IRegistrationContext
            {
                // Configuración universal (serializador, host)
                factoryConfigurator.ConfigureJsonSerializerOptions(opts =>
                {
                    opts.Converters.Add(new JsonStringEnumConverter());
                    return opts;
                });

                string rabbitMqHost = Environment.GetEnvironmentVariable("SYSTEM_MB_HOST");
                string rabbitMqUser = Environment.GetEnvironmentVariable("SYSTEM_MB_USER");
                string rabbitMqPass = Environment.GetEnvironmentVariable("SYSTEM_MB_PASS");

                factoryConfigurator.Host(rabbitMqHost, hostConfigurator =>
                {
                    hostConfigurator.Username(rabbitMqUser);
                    hostConfigurator.Password(rabbitMqPass);
                });

                busConfigurator.AddConsumers(Assembly.GetEntryAssembly()); // addConsumers aquí
                factoryConfigurator.ConfigureEndpoints(context); // El 'context' se pasa directamente desde la lambda de UsingRabbitMq
            });
        });
    }

    /// <summary>
    /// Configura el Message Broker con opciones básicas y permite una configuración personalizada de consumidores/endpoints.
    /// </summary>
    /// <param name="services">Service descriptors collection.</param>
    /// <param name="configuration">System configuration.</param>
    /// <param name="customMassTransitConfig">
    /// Acción para configurar MassTransit (registro de consumidores y endpoints) de forma personalizada.
    /// Recibe el bus registration configurator y el bus factory configurator.
    /// </param>
    /// <returns>Service descriptors collection con servicios personalizados.</returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator, IRabbitMqBusFactoryConfigurator, IRegistrationContext, string> customMassTransitConfig)
    {
        string rabbitMqHost = Environment.GetEnvironmentVariable("SYSTEM_MB_HOST");
        string rabbitMqUser = Environment.GetEnvironmentVariable("SYSTEM_MB_USER");
        string rabbitMqPass = Environment.GetEnvironmentVariable("SYSTEM_MB_PASS");

        var settings = configuration.Get<CustomConfig>();
        string projectName = settings?.Project?.Name ?? "EmptyProjectName";

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.ConfigureJsonSerializerOptions(opts =>
                {
                    opts.Converters.Add(new JsonStringEnumConverter());
                    return opts;
                });

                busFactoryConfigurator.Host(rabbitMqHost, hostConfigurator =>
                {
                    hostConfigurator.Username(rabbitMqUser);
                    hostConfigurator.Password(rabbitMqPass);
                });

                customMassTransitConfig?.Invoke(busConfigurator, busFactoryConfigurator, context, projectName);
            });
        });

        return services;
    }
}