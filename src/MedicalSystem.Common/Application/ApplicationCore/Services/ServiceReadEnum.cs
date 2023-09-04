using System;
using System.Linq;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;
using It270.MedicalSystem.Common.Application.Core.DTOs.Helpers;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services;

/// <summary>
/// Read enum service 
/// </summary>
public class ServiceReadEnum<TEnum> : IServiceReadEnum<TEnum>
    where TEnum : struct
{
    /// <summary>
    /// Get all elements
    /// </summary>
    /// <returns>Process result</returns>
    public CustomWebResponse GetAll()
    {
        var enumData = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(t => new EnumEntityDto<TEnum>(t));

        return new CustomWebResponse()
        {
            ResponseBody = enumData,
        };
    }
}