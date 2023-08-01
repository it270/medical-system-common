using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.Core.Helpers.Storage;
using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;

/// <summary>
/// External storage system interface
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Check existing file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>True if the process is successful and file exists. False otherwise.</returns>
    Task<bool> CheckExistingFile(string fileName, CancellationToken ct = default);

    /// <summary>
    /// Upload file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="file">Upload file data</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    Task<bool> UploadFile(string fileName, IFormFile file, CancellationToken ct = default);

    /// <summary>
    /// Download file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>Downloaded file data. Null otherwise.</returns>
    Task<FileData> DownloadFile(string fileName, CancellationToken ct = default);

    /// <summary>
    /// Delete file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    Task<bool> DeleteFile(string fileName, CancellationToken ct = default);

    /// <summary>
    /// Delete file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="versionId">Version identifier</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    Task<bool> DeleteFile(string fileName, string versionId, CancellationToken ct = default);
}