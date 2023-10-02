using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Application.Core.Helpers.Storage;
using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;

/// <summary>
/// Application file service interface
/// </summary>
public interface IFileServiceBase
{
    /// <summary>
    /// Upload file
    /// </summary>
    /// <param name="name">File name</param>
    /// <param name="file">File data</param>
    /// <param name="hasValidSize">Valid file size flag</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> Upload(string name, IFormFile file, bool hasValidSize = true, CancellationToken ct = default);

    /// <summary>
    /// Download file
    /// </summary>
    /// <param name="name">File name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<FileData> Download(string name, CancellationToken ct = default);

    /// <summary>
    /// Download file with absolute file path
    /// </summary>
    /// <param name="filePath">Absolute file path</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<FileData> DownloadFromRoot(string filePath, CancellationToken ct = default);

    /// <summary>
    /// Delete file
    /// </summary>
    /// <param name="name">File name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> Delete(string name, CancellationToken ct = default);

    /// <summary>
    /// Get file content as string
    /// </summary>
    /// <param name="file">File data</param>
    /// <returns>File content as string</returns>
    string GetString(IFormFile file);
}