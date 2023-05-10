using System;
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
    /// <returns>Service descriptors collection with custom services</returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services,
        IConfiguration configuration)
    {
        string rabbitMqHost = Environment.GetEnvironmentVariable("SYSTEM_MB_HOST");
        string rabbitMqUser = Environment.GetEnvironmentVariable("SYSTEM_MB_USER");
        string rabbitMqPass = Environment.GetEnvironmentVariable("SYSTEM_MB_PASS");

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.Host(rabbitMqHost, hostConfigurator =>
                {
                    hostConfigurator.Username(rabbitMqUser);
                    hostConfigurator.Password(rabbitMqPass);
                });
            });
        });

        return services;
    }
}