using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Controllers.Tools;

/// <summary>
/// Middleware for read access token from query parameter.
/// Original from https://stackoverflow.com/a/53932899
/// </summary>
public class AccessTokenUrlMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor
    /// </summary>
    public AccessTokenUrlMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invoke middleware
    /// </summary>
    /// <param name="context">HTTP context</param>
    public async Task Invoke(HttpContext context)
    {
        // Get access token from query parameter
        var accessToken = context.Request.Query[IamConstants.AccessTokenParamName];

        // Set the authorization header only if it is empty
        if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]) && !string.IsNullOrEmpty(accessToken))
            context.Request.Headers["Authorization"] = $"Bearer {accessToken}";

        await _next(context);
    }
}