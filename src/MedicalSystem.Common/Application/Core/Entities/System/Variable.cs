using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Core.Entities.System;

/// <summary>
/// Variable system entity
/// </summary>
public class Variable : BaseEntity<int>, IAggregateRoot
{
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
    public VariableCategory Category { get; set; }

    /// <summary>
    /// Variable type
    /// </summary>
    public VariableType Type { get; set; }
}