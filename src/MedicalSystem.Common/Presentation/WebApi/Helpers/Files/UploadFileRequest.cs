using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Helpers.Files;

/// <summary>
/// General upload file request
/// </summary>
public class UploadFileRequest
{
    public IFormFile File { get; set; }
}