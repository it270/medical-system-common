using System;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Repositories;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Cache services configuration with dependency injection
/// </summary>
public static class ConfigCacheServices
{
    /// <summary>
    /// Add general cache services
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <param name="configuration">System configuration</param>
    /// <returns>Service descriptors collection with cache services</returns>
    public static IServiceCollection AddCacheServices(this IServiceCollection services,
            IConfiguration configuration)
    {
        var cacheConnectionString = Environment.GetEnvironmentVariable("SYSTEM_CS_CACHE");

        // Configure distributed cache connection
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = cacheConnectionString;

            var settings = configuration.Get<CustomConfig>();
            options.InstanceName = settings?.Project?.Name ?? LogConstants.EmptyProjectName;
        });

        // Configure Redis multiplexer connection
        string hostNameAndPort = GetCsHostAndPort(cacheConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { hostNameAndPort },
                Password = GetCsPassword(cacheConnectionString),
                AbortOnConnectFail = false,
            }));
        services.AddScoped<ICacheRepository, CacheRepository>();

        return services;
    }

    /// <summary>
    /// Get connection string hostname and port
    /// </summary>
    /// <param name="connectionString">Cache connection string</param>
    /// <returns>Cache hostname and port</returns>
    private static string GetCsHostAndPort(string connectionString)
    {
        var csArray = connectionString.Split(',');

        if (csArray == null || csArray.Length <= 0 || string.IsNullOrEmpty(csArray[0]))
            throw new ArgumentException("Invalid cache connection string format.");

        return csArray[0];
    }

    /// <summary>
    /// Get connection string password
    /// </summary>
    /// <param name="connectionString">Cache connection string</param>
    /// <returns>Cache password</returns>
    private static string GetCsPassword(string connectionString)
    {
        var csArray = connectionString.Split("password=");

        if (csArray == null || csArray.Length <= 1 || string.IsNullOrEmpty(csArray[1]))
            return null;

        return csArray[1].Replace(";", string.Empty);
    }
}