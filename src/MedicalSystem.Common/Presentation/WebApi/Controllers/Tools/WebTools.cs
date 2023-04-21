using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Presentation.WebApi.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Controllers.Tools;

public class WebTools<T> : IWebTools
where T : class, IStatusCodeActionResult, IActionResult, new()
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WebTools(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetBaseUrl()
    {
        HttpContext context = _httpContextAccessor.HttpContext;
        return $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}/";
    }

    public IActionResult CustomResponse(CustomWebResponse response)
    {
        if (response.Success)
        {
            var statusCode = (T)Activator.CreateInstance(typeof(T), new object[] { (int)HttpStatusCode.OK, response.ResponseBody });
            return statusCode;
        }
        else
        {
            var statusCode = (T)Activator.CreateInstance(typeof(T), new object[] { (int)response.StatusCode, response });
            return statusCode;
        }
    }
}