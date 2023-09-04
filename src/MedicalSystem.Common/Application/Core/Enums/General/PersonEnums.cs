namespace It270.MedicalSystem.Common.Application.Core.Enums;

/// <summary>
/// Person enums
/// </summary>
public class PersonEnums
{
    /// <summary>
    /// Person birth sex enum
    /// </summary>
    public enum BirthSexEnum : int
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined = 1,

        /// <summary>
        /// Male
        /// </summary>
        Male,

        /// <summary>
        /// Female
        /// </summary>
        Female,

        /// <summary>
        /// Intersexual
        /// </summary>
        Intersexual,
    }

    /// <summary>
    /// Gender identity enum
    /// </summary>
    public enum GenderIdentityEnum : int
    {
        /// <summary>
        /// Undeclared
        /// </summary>
        Undeclared = 1,

        /// <summary>
        /// Male
        /// </summary>
        Male,

        /// <summary>
        /// Female
        /// </summary>
        Female,

        /// <summary>
        /// Neutral
        /// </summary>
        Neutral,

        /// <summary>
        /// Other
        /// </summary>
        Other,

        /// <summary>
        /// Transgender
        /// </summary>
        Transgender,
    }
}