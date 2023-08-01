using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;
using It270.MedicalSystem.Common.Infrastructure.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Storage services configuration with dependency injection
/// </summary>
public static class ConfigStorageServices
{
    /// <summary>
    /// Add all Storage Services
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <param name="configuration">System configuration</param>
    /// <returns>Service descriptors collection with IAM services</returns>
    public static IServiceCollection AddStorageServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IStorageService, StorageService>();

        return services;
    }
}