using It270.MedicalSystem.Common.Application.Core.Interfaces;
using Serilog;

namespace It270.MedicalSystem.Common.Application.Core.DTOs.Helpers;

/// <summary>
/// General entity for Enum values
/// </summary>
/// <typeparam name="TEnum">Enum</typeparam>
public class EnumEntityDto<TEnum> : IDto
    where TEnum : struct
{
    /// <summary>
    /// Constructor
    /// </summary>
    public EnumEntityDto() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Enum value</param>
    public EnumEntityDto(TEnum id)
    {
        Id = id;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Numeric enum value</param>
    public EnumEntityDto(int id)
    {
        if (!typeof(TEnum).IsEnumDefined(id))
        {
            Log.Error($"{id} is not a valid value");
            return;
        }

        Id = (TEnum)(object)id;
    }

    /// <summary>
    /// Enum value
    /// </summary>
    public TEnum Id { get; set; }

    /// <summary>
    /// Enum value as string
    /// </summary>
    public string Name
    {
        get
        {
            if (!typeof(TEnum).IsEnum)
            {
                Log.Error($"{typeof(TEnum)} is not an enum");
                return null;
            }

            return Id.ToString();
        }
    }
}