using System.Collections.Generic;

namespace It270.MedicalSystem.Common.Application.Core.Entities.System;

/// <summary>
/// Variable category entity
/// </summary>
public class VariableCategory : BaseEntity<int>
{
    /// <summary>
    /// Variable category name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Variable category variables
    /// </summary>
    public ICollection<Variable> Variables { get; set; }
}