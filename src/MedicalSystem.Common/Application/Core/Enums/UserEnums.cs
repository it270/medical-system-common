namespace It270.MedicalSystem.Common.Application.Core.Enums;

/// <summary>
/// Enums for users module
/// </summary>
public static class UserEnums
{
    /// <summary>
    /// User role enums (medical records system)
    /// </summary>
    public enum MedicalRecordsRole : int
    {
        /// <summary>
        /// General admin role
        /// </summary>
        Admin = 1,

        /// <summary>
        /// General user role (default role)
        /// </summary>
        User,

        /// <summary>
        /// External system role
        /// </summary>
        ExternalSystem
    }
}