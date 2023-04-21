using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Core.Helpers.General;
using It270.MedicalSystem.Common.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Interfaces.General;

/// <summary>
/// Read data service interface 
/// </summary>
/// <typeparam name="DTO">DTO class type</typeparam>
/// <typeparam name="ET">Entity identifier type</typeparam>
public interface IServiceRead<DTO, ET>
    where DTO : class, IDto
    where ET : notnull
{
    /// <summary>
    /// Get element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> Get(ET id, CancellationToken ct = default);

    /// <summary>
    /// Get all elements
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> GetAll(CancellationToken ct = default);

    /// <summary>
    /// Get elements list (paginated)
    /// </summary>
    /// <param name="skip">Page</param>
    /// <param name="take">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> GetList(int skip, int take, CancellationToken ct = default);
}