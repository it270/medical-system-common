using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static It270.MedicalSystem.Common.Application.Core.Enums.StaticData.StaticDataEnums;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;

/// <summary>
/// Static data module service interface
/// </summary>
public interface IStaticDataService
{
    #region Geographic region functions

    /// <summary>
    /// Validate geographic region
    /// </summary>
    /// <param name="geographicRegionId">Geographic region identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if entity exists. false otherwise</returns>
    Task<bool> ValidateGeographicRegion(int geographicRegionId, CancellationToken ct = default);

    /// <summary>
    /// Get geographic regions
    /// </summary>
    /// <param name="geographicRegionIds">Geographic regions identifiers</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Geographic regions as a dictionary</returns>
    Task<Dictionary<string, string>> GetGeographicRegions(int[] geographicRegionIds, CancellationToken ct = default);

    #endregion

    #region Static data functions

    /// <summary>
    /// Validate static data
    /// </summary>
    /// <param name="staticDataId">Static data identifier</param>
    /// <param name="staticGroup">Static group</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if entity exists. false otherwise</returns>
    Task<bool> ValidateStaticData(int staticDataId, StaticGroupEnum staticGroup, CancellationToken ct = default);

    #endregion
}