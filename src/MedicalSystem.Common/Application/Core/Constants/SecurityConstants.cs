using System.Text.Json;
using static It270.MedicalSystem.Common.Application.Core.Enums.Security.RequestEnums;

namespace It270.MedicalSystem.Common.Application.Core.Constants;

/// <summary>
/// Security system constants
/// </summary>
public static class SecurityConstants
{
    #region Authorization check constants

    /// <summary>
    /// Check permission service url
    /// </summary>
    public const string CheckPermissionUrl = "/Security/CheckPermission";

    /// <summary>
    /// Default JSON deserializer options
    /// </summary>
    public static readonly JsonSerializerOptions JsonDeserializerOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Ignored HTML url paths
    /// </summary>
    public static readonly string[] IgnoredPaths = new string[]
    {
            // Swagger
            "/swagger",
            // Health check
            "/health",
            // Internal services
            "/Internal/",
            // Security services
            "/Security/",
            // General public services
            "/Public/",
            // Document manager MVC templates        
            "/Template/Default/",
            // Static web files
            "/favicon.ico",
            "/img/",
            "/css/",
            "/js/",
    };

    /// <summary>
    /// Ignored HTTP methods
    /// </summary>
    public static readonly RequestTypeEnum[] IgnoredHttpMethods = new RequestTypeEnum[]
    {
            RequestTypeEnum.Head,
            RequestTypeEnum.Options
    };

    #endregion
}