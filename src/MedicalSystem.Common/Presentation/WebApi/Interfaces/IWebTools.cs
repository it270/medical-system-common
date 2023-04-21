using Microsoft.AspNetCore.Mvc;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Interfaces;

public interface IWebTools
{
    /// <summary>
    /// Get base URL 
    /// </summary>
    /// <returns>Current project URL base</returns>
    string GetBaseUrl();

    /// <summary>
    /// Generate custom http response
    /// </summary>
    /// <param name="response">Model with response data</param>
    /// <returns>HTTP response with custom parameters</returns>
    IActionResult CustomResponse(CustomWebResponse response);
}