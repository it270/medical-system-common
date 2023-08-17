using System;
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
        // Patch for discard grpc services
        if (context?.Request?.ContentType == "application/grpc")
        {
            await _next(context);
            return;
        }

        var username = context.GetUserName();
        var controllerName = context.GetControllerName();
        var actionName = context.GetActionName();
        var requestType = context.GetMethod();
        var requestTypeEnum = requestType.CastToEnum<RequestType>();

        using var channel = GrpcChannel.ForAddress(_securityEndpoint);
        var client = new SecurityServiceGrpc.SecurityServiceGrpcClient(channel);

        // Validate response with security microservice
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