using System.Threading;
using System.Threading.Tasks;
using DevExtreme.AspNet.Mvc;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;

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
    /// Check if element exists
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<bool> Exists(ET id, CancellationToken ct = default);

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

    /// <summary>
    /// Get elements list (DevExtreme)
    /// </summary>
    /// <param name="loadOptions">DevExtreme load options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> GetList(DataSourceLoadOptions loadOptions, CancellationToken ct = default);
}