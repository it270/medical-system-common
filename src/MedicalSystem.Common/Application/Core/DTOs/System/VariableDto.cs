using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Core.DTOs.System;

/// <summary>
/// Variable system DTO
/// </summary>
public class VariableDto : IDto
{
    /// <summary>
    /// Entity identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Variable name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Variable description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Variable value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Variable category identifier
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Variable type identifier
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// Variable category
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Variable type
    /// </summary>
    public string Type { get; set; }
}