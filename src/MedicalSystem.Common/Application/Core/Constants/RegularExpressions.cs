namespace It270.MedicalSystem.Common.Application.Core.Constants;

/// <summary>
/// Common regular expression
/// </summary>
public static class RegularExpressions
{
    /// <summary>
    /// Pascal Case regular expression (without numbers)
    /// </summary>
    public const string RegExpPascalCase = "^[A-Z][a-z]*(?:[A-Z][a-z]*)*(?:[A-Z]?)$";
}