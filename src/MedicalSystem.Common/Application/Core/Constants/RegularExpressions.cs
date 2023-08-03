namespace It270.MedicalSystem.Common.Application.Core.Constants;

/// <summary>
/// Common regular expression
/// </summary>
public static class RegularExpressions
{
    #region General

    /// <summary>
    /// Pascal Case regular expression (without numbers)
    /// </summary>
    public const string RegExpPascalCase = "^[A-Z][a-z]*(?:[A-Z][a-z]*)*(?:[A-Z]?)$";

    #endregion

    #region Date and time

    /// <summary>
    /// Time regular expression
    /// Example: "13:37"
    /// (example from https://stackoverflow.com/a/7536768)
    /// </summary>
    public const string RegExpTime = "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$";

    /// <summary>
    /// Time regular expression
    /// Example: "-05:00", "00:00"
    /// (example from https://stackoverflow.com/a/7536768)
    /// </summary>
    public const string RegExpTimeZone = "^(?:(?:[+-](?:1[0-4]|0[1-9]):[0-5][0-9])|00:00)$";


    #endregion
}