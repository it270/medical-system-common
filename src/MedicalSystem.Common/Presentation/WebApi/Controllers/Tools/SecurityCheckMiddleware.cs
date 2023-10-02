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

    /// <summary>
    /// Security endpoint (IAM microservice)
    /// </summary>
    private static readonly string SecurityEndpoint = string.Concat(Environment.GetEnvironmentVariable("MS_IAM_URL"), SecurityConstants.CheckPermissionUrl);

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
        var hasFilePath = SecurityConstants.IgnoredPaths
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

        if (SecurityConstants.IgnoredHttpMethods.Contains(requestTypeEnum))
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

            await MakeForbiddenResponse(context);
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

            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.Error("Security middleware: Response error: {@httpResponse}", new
                {
                    httpResponse.StatusCode,
                    httpResponse.ReasonPhrase,
                    httpResponse.Content,
                });

                await MakeForbiddenResponse(context);
                return;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            response = JsonSerializer.Deserialize<CheckPermissionResponse>(httpResponseContent, GeneralConstants.DefaultJsonDeserializerOpts);
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
            await MakeForbiddenResponse(context);
            return;
        }

        await _next(context);
    }

    /// <summary>
    /// Make http forbidden response
    /// </summary>
    /// <param name="context">Http response</param>
    private async Task MakeForbiddenResponse(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        await context.Response.CompleteAsync();
    }
}