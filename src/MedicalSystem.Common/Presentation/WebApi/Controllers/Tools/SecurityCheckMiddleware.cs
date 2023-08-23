using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Presentation.WebApi.Extensions;
using It270.MedicalSystem.Common.Presentation.WebApi.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Controllers.Tools;

/// <summary>
/// Custom security check middleware
/// </summary>
public class SecurityCheckMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _moduleName;
    private readonly string _securityEndpoint;
    private readonly ILogger _logger;

    /// <summary>
    /// Default constructor
    /// </summary>
    public SecurityCheckMiddleware(IConfiguration configuration,
        RequestDelegate next,
        ILogger logger)
    {
        _securityEndpoint = Environment.GetEnvironmentVariable("SYSTEM_IAM_HOST");
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
        // Discard grpc services
        var grpcContentTypes = new string[]
        {
            "application/grpc",
            "application/grpc-web",
            "application/grpc-web-text",
        };
        if (grpcContentTypes.Contains(context?.Request?.ContentType))
        {
            await _next(context);
            return;
        }

        // Discard anonymous services and static files
        var relativePath = context?.Request?.Path.ToUriComponent();
        var ignoredPaths = new string[]
        {
            "/",                // Default service (swagger)
            "/health",          // Health checks
            "/favicon.ico",     // Favorites icon
        };

        if (ignoredPaths.Contains(relativePath))
        {
            await _next(context);
            return;
        }

        // Discard static files and custom services
        var ignoredFilePaths = new string[]
        {
            "/Email/",           // Email MVC templates
            "/img/",             // static images
        };

        var hasFilePath = ignoredFilePaths
            .Where(p => relativePath.Contains(p))
            .Any();

        if (hasFilePath)
        {
            await _next(context);
            return;
        }

        // Discard HTTP request methods
        var requestType = context.GetMethod();
        var requestTypeEnum = requestType.CastToEnum<RequestType>();
        var ignoredMethods = new RequestType[] { RequestType.Head, RequestType.Options };

        if (ignoredMethods.Contains(requestTypeEnum))
        {
            _logger.Error("GRPC: Invalid request type: {@requestType}", requestType);
            await _next(context);
            return;
        }

        // Validate response with security microservice
        var controllerName = context.GetControllerName();
        var actionName = context.GetActionName();
        var username = context.GetUserName();

        // Proto request data
        var requestData = new CheckPermissionRequest()
        {
            UserName = username ?? string.Empty,
            Module = _moduleName,
            Controller = controllerName ?? string.Empty,
            Action = actionName ?? string.Empty,
            RequestType = requestTypeEnum,
        };

        if (controllerName == null || actionName == null)
        {
            _logger.Error("GRPC: Invalid action values: {@debugData}", new
            {
                Path = relativePath,
                RequestData = requestData,
            });

            await _next(context);
            return;
        }

        // Configure gRPC web mode
        var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
        {
            HttpVersion = HttpVersion.Version11,
        };
        using var channel = GrpcChannel.ForAddress(_securityEndpoint, new()
        {
            HttpClient = new HttpClient(handler),
        });
        var client = new SecurityServiceGrpc.SecurityServiceGrpcClient(channel);
        CheckPermissionReply reply = null;
        var response = false;

        try
        {
            reply = await client.CheckPermissionAsync(requestData);
            response = reply.Response;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "GRPC: Invalid action values: {@debugData}", new
            {
                Path = relativePath,
                RequestData = requestData,
            });
        }

        if (!response)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.CompleteAsync();
            return;
        }

        await _next(context);
    }

}