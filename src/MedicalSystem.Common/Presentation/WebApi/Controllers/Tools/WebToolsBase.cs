using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Presentation.WebApi.Interfaces;
using System.Net;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Controllers.Tools;

/// <summary>
/// Custom web tools
/// </summary>
public abstract class WebToolsBase : ControllerBase, IWebTools
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initialize tools class
    /// </summary>
    public WebToolsBase(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Get base URL 
    /// </summary>
    /// <returns>Current project base URL</returns>
    public virtual string GetBaseUrl()
    {
        HttpContext context = _httpContextAccessor.HttpContext;
        return $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}/";
    }

    /// <summary>
    /// Generate custom http response
    /// </summary>
    /// <param name="response">Model with response data</param>
    /// <returns>HTTP response with custom parameters</returns>
    public virtual IActionResult CustomResponse(CustomWebResponse response)
    {
        if (response.Success)
            return Ok(response.ResponseBody);
        else
        {
            var errorObject = new
            {
                error = response.Message,
                data = response.ResponseBody,
            };

            return StatusCode((int)response.StatusCode, errorObject);
        }
    }
}