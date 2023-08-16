using It270.MedicalSystem.Common.Presentation.WebApi.Controllers.Tools;
using Microsoft.AspNetCore.Builder;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Extensions;

/// <summary>
/// Extensions for custom security check middleware
/// </summary>
public static class SecurityCheckMiddlewareExtensions
{
    /// <summary>
    /// Use custom security check middleware
    /// </summary>
    public static IApplicationBuilder UseCustomSecurityMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityCheckMiddleware>();
    }
}