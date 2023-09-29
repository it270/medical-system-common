using static It270.MedicalSystem.Common.Application.Core.Enums.Security.RequestEnums;

namespace It270.MedicalSystem.Common.Application.Core.Helpers.Security;

/// <summary>
/// Check permissions by request
/// </summary>
public class CheckPermissionRequest
{
    /// <summary>
    /// Current username
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Request module
    /// </summary>
    public string Module { get; set; }

    /// <summary>
    /// Request controller
    /// </summary>
    public string Controller { get; set; }

    /// <summary>
    /// Request action
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// Request type
    /// </summary>
    public RequestTypeEnum RequestType { get; set; }
}