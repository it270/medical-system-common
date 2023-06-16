using System.Text.RegularExpressions;
using It270.MedicalSystem.Common.Application.Core.Constants;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services;

/// <summary>
/// IAM common tools
/// </summary>
public static class IamTools
{
    #region Validators

    /// <summary>
    /// Check user name format
    /// </summary>
    /// <param name="userName">User name</param>
    /// <returns>True if format is correct. False otherwise</returns>
    public static bool IsAValidUserName(string userName)
    {
        return Regex.Match(userName, RegularExpressions.RegExpPascalCase).Success;
    }

    /// <summary>
    /// Check user name format
    /// </summary>
    /// <param name="roleName">Role name</param>
    /// <returns>True if format is correct. False otherwise</returns>
    public static bool IsAValidRoleName(string roleName)
    {
        return IsAValidUserName(roleName);
    }

    #endregion
}