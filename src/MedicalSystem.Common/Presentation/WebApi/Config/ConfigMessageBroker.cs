// EN It270.MedicalSystem.Common/Presentation/WebApi/Config/ConfigMessageBroker.cs
using System;
using System.Text.Json.Serialization;
using It270.MedicalSystem.Common.Application.Core.Helpers.General; // Para CustomConfig
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Configuración de servicios del Message Broker con inyección de dependencias.
/// </summary>
public static class ConfigMessageBroker
{
    /// <summary>
    /// Estructura interna para contener las credenciales de RabbitMQ.
    /// </summary>
    private record RabbitMqCredentials(string Host, string User, string Pass);

    /// <summary>
    /// Método auxiliar privado para obtener las credenciales de RabbitMQ de forma centralizada.
    /// </summary>
    /// <param name="configuration">Configuración del sistema.</param>
    /// <returns>Objeto RabbitMqCredentials con los datos de conexión.</returns>
    /// <exception cref="InvalidOperationException">Si faltan las variables de entorno para RabbitMQ.</exception>
    private static RabbitMqCredentials GetRabbitMqCredentials(IConfiguration configuration)
    {
        string rabbitMqHost = Environment.GetEnvironmentVariable("SYSTEM_MB_HOST") ?? configuration["RabbitMq:Host"];
        string rabbitMqUser = Environment.GetEnvironmentVariable("SYSTEM_MB_USER") ?? configuration["RabbitMq:User"];
        string rabbitMqPass = Environment.GetEnvironmentVariable("SYSTEM_MB_PASS") ?? configuration["RabbitMq:Pass"];

        if (string.IsNullOrEmpty(rabbitMqHost) || string.IsNullOrEmpty(rabbitMqUser) || string.IsNullOrEmpty(rabbitMqPass))
        {
            throw new InvalidOperationException("Las variables de entorno o la configuración para RabbitMQ (Host, User, Pass) no están definidas.");
        }
        return new RabbitMqCredentials(rabbitMqHost, rabbitMqUser, rabbitMqPass);
    }

    /// <summary>
    /// Método auxiliar privado para configurar el host de RabbitMQ y el serializador JSON.
    /// </summary>
    private static void ConfigureBaseRabbitMqHostAndSerializer(
        IRabbitMqBusFactoryConfigurator busFactoryConfigurator,
        IConfiguration configuration)
    {
        var credentials = GetRabbitMqCredentials(configuration);

        busFactoryConfigurator.ConfigureJsonSerializerOptions(opts =>
        {
            opts.Converters.Add(new JsonStringEnumConverter());
            return opts;
        });

        busFactoryConfigurator.Host(credentials.Host, hostConfigurator =>
        {
            hostConfigurator.Username(credentials.User);
            hostConfigurator.Password(credentials.Pass);
        });
    }

    /// <summary>
    /// **MÉTODO EXISTENTE - IMPLEMENTACIÓN MODIFICADA PARA COMPATIBILIDAD HACIA ATRÁS.**
    /// Configura el Message Broker (MassTransit con RabbitMQ) para microservicios que
    /// principalmente envían mensajes o para aquellos que usaban la configuración de auto-descubrimiento
    /// sin un control granular de consumidores. Este método ya no configura endpoints de recepción
    /// automáticamente para evitar conflictos en el ServiceCollection.
    /// </summary>
    /// <param name="services">Colección de descriptores de servicio.</param>
    /// <param name="configuration">Configuración del sistema.</param>
    /// <returns>Colección de descriptores de servicio con servicios personalizados.</returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        // El projectName es solo necesario si configuras el KebabCaseEndpointNameFormatter con él.
        // Si no lo usas más allá de eso, podríamos incluso omitirlo de aquí si el formatter se configura de forma estándar.
        // Pero lo mantenemos para coherencia si hubiera otros usos.
        var settings = configuration.Get<CustomConfig>();
        string projectName = settings?.Project?.Name ?? "EmptyProjectName";

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
            {
                // Llamamos al método auxiliar para la configuración base de RabbitMQ
                ConfigureBaseRabbitMqHostAndSerializer(busFactoryConfigurator, configuration);

                // ¡IMPORTANTE! Eliminamos:
                // busConfigurator.AddConsumers(Assembly.GetEntryAssembly());
                // busFactoryConfigurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(projectName, false));
            });
        });

        return services;
    }

    /// <summary>
    /// **NUEVO MÉTODO PARA MICROSERVICIOS CON CONSUMIDORES ESPECÍFICOS.**
    /// Configura el Message Broker (MassTransit con RabbitMQ) y permite la configuración
    /// explícita de consumidores y sus receive endpoints.
    /// </summary>
    /// <param name="services">Colección de descriptores de servicio.</param>
    /// <param name="configuration">Configuración del sistema.</param>
    /// <param name="configureSpecificConsumers">
    /// Una acción para configurar los consumidores y los receive endpoints específicos del microservicio.
    /// Recibe el bus registration configurator, el bus factory configurator, el registration context y el nombre del proyecto.
    /// </param>
    /// <returns>Colección de descriptores de servicio con servicios personalizados.</returns>
    public static IServiceCollection AddMessageBrokerWithSpecificConsumers(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator, IRabbitMqBusFactoryConfigurator, IRegistrationContext, string> configureSpecificConsumers)
    {
        var settings = configuration.Get<CustomConfig>();
        string projectName = settings?.Project?.Name ?? "EmptyProjectName";

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
            {
                // Llamamos al método auxiliar para la configuración base de RabbitMQ
                ConfigureBaseRabbitMqHostAndSerializer(busFactoryConfigurator, configuration);

                // Aquí se invoca la acción que viene del microservicio para su configuración específica de consumidores/endpoints
                configureSpecificConsumers?.Invoke(busConfigurator, busFactoryConfigurator, context, projectName);
            });
        });

        return services;
    }
}