using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Core.Helpers.General;

namespace It270.MedicalSystem.Common.Application.Interfaces.General;

/// <summary>
/// Disable service interface
/// </summary>
/// <typeparam name="ET">Entity identifier type</typeparam>
public interface IServiceDisable<ET>
    where ET : notnull
{
    /// <summary>
    /// Disable or enable element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="disable">Disable flag</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> Disable(ET id, bool disable, CancellationToken ct = default);
}