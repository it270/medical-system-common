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

    public WebToolsBase(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public virtual string GetBaseUrl()
    {
        HttpContext context = _httpContextAccessor.HttpContext;
        return $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}/";
    }

    public virtual IActionResult CustomResponse(CustomWebResponse response)
    {
        if (response.Success)
            return StatusCode((int)HttpStatusCode.OK, response.ResponseBody);
        else
            return StatusCode((int)response.StatusCode, response.ResponseBody);
    }
}