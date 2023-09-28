using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Application.Core.Helpers.Security;
using It270.MedicalSystem.Common.Presentation.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using static It270.MedicalSystem.Common.Application.Core.Constants.MediaTypeNames;
using static It270.MedicalSystem.Common.Application.Core.Enums.Security.RequestEnums;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Controllers.Tools;

/// <summary>
/// Custom security check middleware
/// </summary>
public class SecurityCheckMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _moduleName;
    private readonly ILogger _logger;

    #region Constants

    /// <summary>
    /// Security endpoint (IAM microservice)
    /// </summary>
    private static readonly string SecurityEndpoint = string.Concat(Environment.GetEnvironmentVariable("SYSTEM_SECURITY_CHECK_URL"), "/Security/CheckPermission");

    /// <summary>
    /// Default JSON deserializer options
    /// </summary>
    private static readonly JsonSerializerOptions JsonDeserializerOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Ignored HTML url paths
    /// </summary>
    private static readonly string[] IgnoredPaths = new string[]
    {
        // Default service (swagger)
        "/",
        // Health checks
        "/health",
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
    private static readonly RequestTypeEnum[] IgnoredHttpMethods = new RequestTypeEnum[]
    {
        RequestTypeEnum.Head,
        RequestTypeEnum.Options
    };

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public SecurityCheckMiddleware(IConfiguration configuration,
        RequestDelegate next,
        ILogger logger)
    {
        var settings = configuration.Get<CustomConfig>();
        _moduleName = settings?.Project?.ModuleName ?? LogConstants.EmptyModuleName;
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invoke middleware
    /// </summary>
    /// <param name="context">HTTP context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var relativePath = context?.Request?.Path.ToUriComponent();

        // Discard static files and custom services
        var hasFilePath = IgnoredPaths
            .Where(p => relativePath.Contains(p))
            .Any();

        if (hasFilePath)
        {
            await _next(context);
            return;
        }

        // Discard HTTP request methods
        var requestType = context.GetMethod();
        var requestTypeEnum = requestType.CastToEnum<RequestTypeEnum>();

        if (IgnoredHttpMethods.Contains(requestTypeEnum))
        {
            await _next(context);
            return;
        }

        // Validate response with security microservice
        var controllerName = context.GetControllerName();
        var actionName = context.GetActionName();
        var username = context.GetUserName();

        // Build request data
        var requestData = new CheckPermissionRequest()
        {
            UserName = username,
            Module = _moduleName,
            Controller = controllerName,
            Action = actionName,
            RequestType = requestTypeEnum,
        };

        if (controllerName == null || actionName == null)
        {
            _logger.Error("Security middleware: Invalid action values: {@debugData}", new
            {
                Path = relativePath,
                RequestData = requestData,
            });

            await _next(context);
            return;
        }

        // Validate current request permissions
        CheckPermissionResponse response = null;
        try
        {
            var client = new HttpClient();
            var requestDataJson = JsonSerializer.Serialize(requestData);
            var content = new StringContent(requestDataJson, Encoding.UTF8, Data.Json);
            var httpResponse = await client.PostAsync(SecurityEndpoint, content);
            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            response = JsonSerializer.Deserialize<CheckPermissionResponse>(httpResponseContent, JsonDeserializerOpts);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Security middleware: Exception. Request data: {@debugData}", new
            {
                Path = relativePath,
                RequestData = requestData,
            });
        }

        if (!(response?.Authorized ?? false))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.CompleteAsync();
            return;
        }

        await _next(context);
    }
}