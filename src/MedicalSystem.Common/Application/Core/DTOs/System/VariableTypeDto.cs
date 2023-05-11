using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Core.DTOs.System;

/// <summary>
/// Variable type DTO
/// </summary>
public class VariableTypeDto : IDto
{
    /// <summary>
    /// Entity identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Variable type name
    /// </summary>
    public string Name { get; set; }
}