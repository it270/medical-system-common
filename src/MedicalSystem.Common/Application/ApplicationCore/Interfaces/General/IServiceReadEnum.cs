using It270.MedicalSystem.Common.Application.Core.Helpers.General;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;

/// <summary>
/// Read enum service interface 
/// </summary>
public interface IServiceReadEnum<TEnum>
    where TEnum : struct
{
    /// <summary>
    /// Get all elements
    /// </summary>
    /// <returns>Process result</returns>
    CustomWebResponse GetAll();
}