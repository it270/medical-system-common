using System;
using System.Threading.Tasks;

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
    /// <param name="language">Language abbreviation</param>
    /// <returns>Translated key</returns>
    Task<string> GetString<KeyEnum>(KeyEnum key, string language) where KeyEnum : Enum;
}