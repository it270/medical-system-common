using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;
using It270.MedicalSystem.Common.Application.ApplicationCore.Services;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Common services configuration with dependency injection
/// </summary>
public static class ConfigCommonServices
{
    /// <summary>
    /// Add all common services
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <param name="configuration">System configuration</param>
    /// <returns>Service descriptors collection with common services</returns>
    public static async Task<IServiceCollection> AddCommonServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Enable HTTP client
        services.AddHttpClient();

        await services.AddAuthService(configuration);
        services.AddCommonWebServices();
        services.AddEnumServices();
        services.AddStaticDataServices();

        return services;
    }

    /// <summary>
    /// Add authentication service
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <param name="configuration">System configuration</param>
    /// <returns>Service descriptors collection with authentication service</returns>
    public static async Task<IServiceCollection> AddAuthService(this IServiceCollection services,
        IConfiguration configuration)
    {
        // AWS code from https://stackoverflow.com/a/71871177 and https://stackoverflow.com/a/72974164

        var region = Environment.GetEnvironmentVariable("AWS_COGNITO_REGION");
        var userPoolId = Environment.GetEnvironmentVariable("AWS_COGNITO_DEFAULT_POOL");
        var validIssuer = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
        var audience = Environment.GetEnvironmentVariable("AWS_COGNITO_CLIENT_ID");

        var httpClient = new HttpClient();
        var webKeySetJson = await httpClient.GetStringAsync($"{validIssuer}/.well-known/jwks.json");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = validIssuer,
                    IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                    {
                        var jsonWebKeySet = new JsonWebKeySet(webKeySetJson);
                        return jsonWebKeySet?.GetSigningKeys();
                    },
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    AudienceValidator = (audiences, securityToken, validationParameters) =>
                    {
                        //This is necessary because Cognito tokens doesn't have "aud" claim. Instead the audience is set in "client_id"
                        var castedToken = securityToken as JwtSecurityToken;
                        var hasAud = castedToken.Claims
                            .Any(a => a.Type == IamConstants.Aud && a.Value == audience);
                        return hasAud;
                    },
                    ValidAudience = audience,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = IamConstants.Role
                };
            });

        return services;
    }

    /// <summary>
    /// Add common web services
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddCommonWebServices(this IServiceCollection services)
    {
        services.AddScoped<IDocumentManagerTools, DocumentManagerTools>();
        return services;
    }

    /// <summary>
    /// Configure enum services
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddEnumServices(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IServiceReadEnum<>), typeof(ServiceReadEnum<>));
        return services;
    }

    /// <summary>
    /// Configure static data services
    /// </summary>
    /// <param name="services">Service descriptors collection</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddStaticDataServices(this IServiceCollection services)
    {
        services.AddScoped<IStaticDataService, StaticDataService>();
        return services;
    }
}