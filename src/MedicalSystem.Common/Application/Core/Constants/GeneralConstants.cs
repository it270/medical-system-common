using System.Text.Json;

namespace It270.MedicalSystem.Common.Application.Core.Constants;

/// <summary>
/// General system constants
/// </summary>
public static class GeneralConstants
{
    #region System initialization

    /// <summary>
    /// Default administrator user name
    /// </summary>
    public const string SuperAdminUserName = "Admin";

    #endregion

    #region General

    /// <summary>
    /// Default JSON deserializer options
    /// </summary>
    public static readonly JsonSerializerOptions DefaultJsonDeserializerOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    #endregion
}