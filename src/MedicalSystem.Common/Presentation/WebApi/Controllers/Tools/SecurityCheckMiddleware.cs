using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Grpc.Net.Client;
using It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Presentation.WebApi.Extensions;
using It270.MedicalSystem.Common.Presentation.WebApi.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Controllers.Tools;

/// <summary>
/// Custom security check middleware
/// </summary>
public class SecurityCheckMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _moduleName;
    private readonly string _securityEndpoint;

    /// <summary>
    /// Default constructor
    /// </summary>
    public SecurityCheckMiddleware(IConfiguration configuration,
        RequestDelegate next)
    {
        _securityEndpoint = Environment.GetEnvironmentVariable("SYSTEM_IAM_HOST");
        var settings = configuration.Get<CustomConfig>();
        _moduleName = settings?.Project?.ModuleName ?? LogConstants.EmptyModuleName;
        _next = next;
    }

    /// <summary>
    /// Invoke middleware
    /// </summary>
    /// <param name="context">HTTP context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // Discard grpc services
        if (context?.Request?.ContentType == "application/grpc")
        {
            await _next(context);
            return;
        }

        // Discard anonymous services and static files
        var relativePath = context?.Request?.Path.ToUriComponent();
        var ignoredPaths = new string[] {
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
        var ignoredFilePaths = new string[] {
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
            await _next(context);
            return;
        }

        // Validate response with security microservice
        var controllerName = context.GetControllerName();
        var actionName = context.GetActionName();
        var username = context.GetUserName();

        using var channel = GrpcChannel.ForAddress(_securityEndpoint);
        var client = new SecurityServiceGrpc.SecurityServiceGrpcClient(channel);

        var reply = await client.CheckPermissionAsync(
            new()
            {
                UserName = username,
                Module = _moduleName,
                Controller = controllerName,
                Action = actionName,
                RequestType = requestTypeEnum,
            });

        var response = reply.Response;

        if (!response)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.CompleteAsync();
            return;
        }

        await _next(context);
    }

}