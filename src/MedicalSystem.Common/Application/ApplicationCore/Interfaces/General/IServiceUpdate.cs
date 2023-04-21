using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;

/// <summary>
/// Update service interface
/// </summary>
/// <typeparam name="DTO">DTO class type</typeparam>
/// <typeparam name="ET">Entity identifier type</typeparam>
public interface IServiceUpdate<DTO, ET>
    where DTO : class, IDto
    where ET : notnull
{
    /// <summary>
    /// Update element
    /// </summary>
    /// <param name="id">Element identifier</param>
    /// <param name="entityData">Entity data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    Task<CustomWebResponse> Update(ET id, DTO entityData, CancellationToken ct = default);
}