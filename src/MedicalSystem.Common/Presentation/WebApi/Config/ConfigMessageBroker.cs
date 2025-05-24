// EN It270.MedicalSystem.Common/Presentation/WebApi/Config/ConfigMessageBroker.cs
using System;
using System.Text.Json.Serialization;
using It270.MedicalSystem.Common.Application.Core.Helpers.General; // Para CustomConfig
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Configuraci�n de servicios del Message Broker con inyecci�n de dependencias.
/// </summary>
public static class ConfigMessageBroker
{
    /// <summary>
    /// Estructura interna para contener las credenciales de RabbitMQ.
    /// </summary>
    private record RabbitMqCredentials(string Host, string User, string Pass);

    /// <summary>
    /// M�todo auxiliar privado para obtener las credenciales de RabbitMQ de forma centralizada.
    /// </summary>
    /// <param name="configuration">Configuraci�n del sistema.</param>
    /// <returns>Objeto RabbitMqCredentials con los datos de conexi�n.</returns>
    /// <exception cref="InvalidOperationException">Si faltan las variables de entorno para RabbitMQ.</exception>
    private static RabbitMqCredentials GetRabbitMqCredentials(IConfiguration configuration)
    {
        string rabbitMqHost = Environment.GetEnvironmentVariable("SYSTEM_MB_HOST") ?? configuration["RabbitMq:Host"];
        string rabbitMqUser = Environment.GetEnvironmentVariable("SYSTEM_MB_USER") ?? configuration["RabbitMq:User"];
        string rabbitMqPass = Environment.GetEnvironmentVariable("SYSTEM_MB_PASS") ?? configuration["RabbitMq:Pass"];

        if (string.IsNullOrEmpty(rabbitMqHost) || string.IsNullOrEmpty(rabbitMqUser) || string.IsNullOrEmpty(rabbitMqPass))
        {
            throw new InvalidOperationException("Las variables de entorno o la configuraci�n para RabbitMQ (Host, User, Pass) no est�n definidas.");
        }
        return new RabbitMqCredentials(rabbitMqHost, rabbitMqUser, rabbitMqPass);
    }

    /// <summary>
    /// M�todo auxiliar privado para configurar el host de RabbitMQ y el serializador JSON.
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
    /// **M�TODO EXISTENTE - IMPLEMENTACI�N MODIFICADA PARA COMPATIBILIDAD HACIA ATR�S.**
    /// Configura el Message Broker (MassTransit con RabbitMQ) para microservicios que
    /// principalmente env�an mensajes o para aquellos que usaban la configuraci�n de auto-descubrimiento
    /// sin un control granular de consumidores. Este m�todo ya no configura endpoints de recepci�n
    /// autom�ticamente para evitar conflictos en el ServiceCollection.
    /// </summary>
    /// <param name="services">Colecci�n de descriptores de servicio.</param>
    /// <param name="configuration">Configuraci�n del sistema.</param>
    /// <returns>Colecci�n de descriptores de servicio con servicios personalizados.</returns>
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        // El projectName es solo necesario si configuras el KebabCaseEndpointNameFormatter con �l.
        // Si no lo usas m�s all� de eso, podr�amos incluso omitirlo de aqu� si el formatter se configura de forma est�ndar.
        // Pero lo mantenemos para coherencia si hubiera otros usos.
        var settings = configuration.Get<CustomConfig>();
        string projectName = settings?.Project?.Name ?? "EmptyProjectName";

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
            {
                // Llamamos al m�todo auxiliar para la configuraci�n base de RabbitMQ
                ConfigureBaseRabbitMqHostAndSerializer(busFactoryConfigurator, configuration);

                // �IMPORTANTE! Eliminamos:
                // busConfigurator.AddConsumers(Assembly.GetEntryAssembly());
                // busFactoryConfigurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(projectName, false));
            });
        });

        return services;
    }

    /// <summary>
    /// **NUEVO M�TODO PARA MICROSERVICIOS CON CONSUMIDORES ESPEC�FICOS.**
    /// Configura el Message Broker (MassTransit con RabbitMQ) y permite la configuraci�n
    /// expl�cita de consumidores y sus receive endpoints.
    /// </summary>
    /// <param name="services">Colecci�n de descriptores de servicio.</param>
    /// <param name="configuration">Configuraci�n del sistema.</param>
    /// <param name="configureSpecificConsumers">
    /// Una acci�n para configurar los consumidores y los receive endpoints espec�ficos del microservicio.
    /// Recibe el bus registration configurator, el bus factory configurator, el registration context y el nombre del proyecto.
    /// </param>
    /// <returns>Colecci�n de descriptores de servicio con servicios personalizados.</returns>
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
                // Llamamos al m�todo auxiliar para la configuraci�n base de RabbitMQ
                ConfigureBaseRabbitMqHostAndSerializer(busFactoryConfigurator, configuration);

                // Aqu� se invoca la acci�n que viene del microservicio para su configuraci�n espec�fica de consumidores/endpoints
                configureSpecificConsumers?.Invoke(busConfigurator, busFactoryConfigurator, context, projectName);
            });
        });

        return services;
    }
}