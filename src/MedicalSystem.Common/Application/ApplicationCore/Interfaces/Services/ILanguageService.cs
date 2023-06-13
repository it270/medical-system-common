using System;
using System.Threading;
using System.Threading.Tasks;
using static It270.MedicalSystem.Common.Application.Core.Enums.LanguageEnums;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;

/// <summary>
/// Language service interface
/// </summary>
public interface ILanguageService
{
    /// <summary>
    /// Get string from key and language
    /// </summary>
    /// <typeparam name="KeyEnum">String key enum</typeparam>
    /// <param name="key">String key</param>
    /// <param name="language">Language enum value</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Translated key</returns>
    Task<string> GetString<KeyEnum>(KeyEnum key, LanguageEnum language, CancellationToken ct = default)
    where KeyEnum : Enum;
}