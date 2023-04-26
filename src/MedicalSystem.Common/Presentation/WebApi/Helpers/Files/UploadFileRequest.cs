using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Helpers.Files;

/// <summary>
/// General upload file request
/// </summary>
public class UploadFileRequest
{
    /// <summary>
    /// General file
    /// </summary>
    public IFormFile File { get; set; }
}