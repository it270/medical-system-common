namespace It270.MedicalSystem.Common.Application.Core.Constants;

/// <summary>
/// Constants for Identity and Access Management
/// </summary>
public static class IamConstants
{
    #region General

    /// <summary>
    /// Access token query parameter name
    /// </summary>
    public const string AccessTokenParamName = "access_token";

    #endregion

    #region User attributes

    /// <summary>
    /// Audience input name
    /// </summary>
    public const string Aud = "client_id";

    /// <summary>
    /// User name input name
    /// </summary>
    public const string UserName = "username";

    /// <summary>
    /// Subject input name
    /// </summary>
    public const string Sub = "sub";

    /// <summary>
    /// Role input name
    /// </summary>
    public const string Role = "cognito:groups";

    /// <summary>
    /// Email input name
    /// </summary>
    public const string Email = "email";

    /// <summary>
    /// Phone number input name
    /// </summary>
    public const string PhoneNumber = "phone_number";

    #endregion
}