using System;

namespace It270.MedicalSystem.Common.Application.Core.Helpers.UserNS;

/// <summary>
/// IAM User data (Cognito)
/// </summary>
public class IamUser
{
    /// <summary>
    /// User Subject
    /// </summary>
    public string Sub { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// User enabled flag
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// User email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User phone number
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// User creation date
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// User last edition date
    /// </summary>
    public DateTime EditionDate { get; set; }
}