namespace It270.MedicalSystem.Common.Application.Core.Constants;

/// <summary>
/// Constants for Identity and Access Management
/// </summary>
public static class IamConstants
{
    #region User attributes

    public const string Audience = "client_id";
    public const string UserName = "username";
    public const string Sub = "sub";
    public const string Role = "cognito:groups";
    public const string Email = "email";
    public const string PhoneNumber = "phone_number";

    #endregion
}