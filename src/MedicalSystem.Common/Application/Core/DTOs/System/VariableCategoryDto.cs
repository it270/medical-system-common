using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Core.DTOs.System;

/// <summary>
/// Variable category DTO
/// </summary>
public class VariableCategoryDto : IDto
{
    /// <summary>
    /// Entity identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Variable category name
    /// </summary>
    public string Name { get; set; }
}