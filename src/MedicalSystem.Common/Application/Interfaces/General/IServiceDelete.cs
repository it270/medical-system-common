using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Core.Helpers.General;

namespace It270.MedicalSystem.Common.Application.Interfaces.General;

/// <summary>
/// Delete service interface
/// </summary>
/// <typeparam name="ET">Entity identifier type</typeparam>
public interface IServiceDelete<ET>
    where ET : notnull
{
    /// <summary>
    /// Delete element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> Delete(ET id, CancellationToken ct = default);
}