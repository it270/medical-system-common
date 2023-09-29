namespace It270.MedicalSystem.Common.Application.Core.Enums.Security;

/// <summary>
/// General request enums
/// </summary>
public static class RequestEnums
{
    /// <summary>
    /// Request type enum
    /// </summary>
    public enum RequestTypeEnum : int
    {
        /// <summary>
        /// GET request
        /// </summary>
        Get,

        /// <summary>
        /// POST request
        /// </summary>
        Post,

        /// <summary>
        /// PUT request
        /// </summary>
        Put,

        /// <summary>
        /// DELETE request
        /// </summary>
        Delete,

        /// <summary>
        /// PATCH request
        /// </summary>
        Patch,

        /// <summary>
        /// HEAD request
        /// </summary>
        Head,

        /// <summary>
        /// OPTIONS request
        /// </summary>
        Options,
    }
}