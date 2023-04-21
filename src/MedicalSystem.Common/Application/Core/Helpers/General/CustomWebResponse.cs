using System.Net;

namespace It270.MedicalSystem.Common.Application.Core.Helpers.General;

/// <summary>
/// Custom HTTP response model
/// </summary>
public class CustomWebResponse
{

    public CustomWebResponse(bool error = false)
    {
        Success = !error;
        StatusCode = (error ? HttpStatusCode.BadRequest : HttpStatusCode.OK);
    }

    public bool Success { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public object ResponseBody { get; set; }
    public string Message { get; set; }
}