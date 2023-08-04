using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;

/// <summary>
/// Storage setup interface
/// </summary>
/// <typeparam name="ET">Entity identifier type</typeparam>
public interface IServiceStorage<ET>
    where ET : notnull
{
    /// <summary>
    /// Storage entity prefix
    /// </summary>
    string StoragePrefix { get; }

    /// <summary>
    /// Get entity file
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> GetFile(ET id, CancellationToken ct = default);

    /// <summary>
    /// Upload entity file
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <param name="file">File data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> UploadFile(ET id, IFormFile file, CancellationToken ct = default);
}