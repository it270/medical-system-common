using System;
using static It270.MedicalSystem.Common.Application.Core.Enums.LanguageEnums;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;

/// <summary>
/// General language extensions
/// </summary>
public static class LanguageExtensions
{
    /// <summary>
    /// Check valid language
    /// </summary>
    /// <param name="languageStr">Language input (string)</param>
    /// <returns>True if is a valid language. False otherwise</returns>
    public static bool IsAValidLanguage(this string languageStr)
    {
        return Enum.TryParse<LanguageEnum>(languageStr, true, out _);
    }
}