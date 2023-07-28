using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Helpers.Files;

/// <summary>
/// General upload file request
/// </summary>
public class UploadFileRequest
{
    /// <summary>
    /// General entity identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// File name (optional)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// File description (optional)
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// General file
    /// </summary>
    public IFormFile File { get; set; }
}