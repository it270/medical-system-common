using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Repositories;

/// <summary>
/// Cache database repository
/// </summary>
public interface ICacheRepository
{
    /// <summary>
    /// Remove keys by pattern
    /// </summary>
    /// <param name="keyPattern">Key pattern</param>
    /// <param name="ct">Cancellation token</param>
    Task RemoveWithWildCardAsync(string keyPattern, CancellationToken ct = default);

    /// <summary>
    /// Get keys by pattern
    /// </summary>
    /// <param name="pattern">Key pattern</param>
    /// <returns>Keys list</returns>
    IAsyncEnumerable<string> GetKeysAsync(string pattern);
}