using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Core.DTOs.UserNS;

/// <summary>
/// New user request helper
/// </summary>
/// <typeparam name="T">User DTO entity type</typeparam>
public class NewUserRequest<T>
where T : IDto
{
    /// <summary>
    /// Request language
    /// </summary>
    public string Language { get; set; }

    /// <summary>
    /// Entity data
    /// </summary>
    public T EntityData { get; set; }
}