using System.Net;

namespace It270.MedicalSystem.Common.Application.Core.Helpers.General;

/// <summary>
/// Custom HTTP response model
/// </summary>
public class CustomWebResponse
{
    /// <summary>
    /// Default response constructor
    /// </summary>
    /// <param name="error">Error boolean flag</param>
    public CustomWebResponse(bool error = false)
    {
        Success = !error;
        StatusCode = (error ? HttpStatusCode.BadRequest : HttpStatusCode.OK);
    }

    /// <summary>
    /// Response success code
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Response status code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Custom response body
    /// </summary>
    public object ResponseBody { get; set; }

    /// <summary>
    /// Custom response error message
    /// </summary>
    public string Message { get; set; }
}