using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Extensions;

/// <summary>
/// General HTTP context extensions
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Get controller name
    /// </summary>
    /// <param name="httpContext">Current HTTP Context</param>
    /// <returns>Controller name</returns>
    public static string GetControllerName(this HttpContext httpContext)
    {
        var controllerName = httpContext.Request.RouteValues["controller"].ToString();
        return controllerName;
    }

    /// <summary>
    /// Get action name
    /// </summary>
    /// <param name="httpContext">Current HTTP Context</param>
    /// <returns>Controller action name</returns>
    public static string GetActionName(this HttpContext httpContext)
    {
        var actionName = httpContext.Request.RouteValues["action"].ToString();
        return actionName;
    }

    /// <summary>
    /// Get request method
    /// </summary>
    /// <param name="httpContext">Current HTTP Context</param>
    /// <returns>Method name</returns>
    public static string GetMethod(this HttpContext httpContext)
    {
        var actionName = httpContext.Request.Method;
        return actionName;
    }

    /// <summary>
    /// Get claims from access token
    /// </summary>
    /// <param name="token">Access token string</param>
    /// <returns>Claims list</returns>
    private static IEnumerable<Claim> GetTokenClaims(string token)
    {
        var securityTokenHandler = new JwtSecurityTokenHandler();

        if (securityTokenHandler.CanReadToken(token))
        {
            var decriptedToken = securityTokenHandler.ReadJwtToken(token);
            return decriptedToken.Claims;
        }

        return null;
    }

    /// <summary>
    /// Get username
    /// </summary>
    /// <param name="httpContext">Current HTTP Context</param>
    /// <returns>User name</returns>
    public static string GetUserName(this HttpContext httpContext)
    {
        var username = httpContext.User.FindAll(IamConstants.UserName);

        return username.FirstOrDefault()?.Value;
    }

    /// <summary>
    /// Get user roles
    /// </summary>
    /// <param name="httpContext">Current HTTP Context</param>
    /// <returns>User roles list</returns>
    public static string[] GetRoles(this HttpContext httpContext)
    {
        var roles = httpContext.User.FindAll(IamConstants.Role);

        return roles
            .Select(r => r.Value)
            .ToArray();
    }

    /// <summary>
    /// Check if current session user is authenticated
    /// </summary>
    /// <param name="httpContext">HTTP context</param>
    /// <returns>True if user is authenticated. False otherwise</returns>
    public static bool IsAuthenticated(this HttpContext httpContext)
    {
        return httpContext.User.Identity.IsAuthenticated;
    }

    /// <summary>
    /// Check if the context has a valid session
    /// </summary>
    /// <param name="httpContext">HTTP context</param>
    /// <returns>True if the context has a valid session. False otherwise</returns>
    public static bool IsAValidSession(this HttpContext httpContext)
    {
        return !string.IsNullOrEmpty(httpContext.GetUserName()) && httpContext.GetRoles().Any() && httpContext.IsAuthenticated();
    }
}