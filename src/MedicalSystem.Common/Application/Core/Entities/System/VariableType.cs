using System.Collections.Generic;

namespace It270.MedicalSystem.Common.Application.Core.Entities.System;

/// <summary>
/// Variable type
/// </summary>
public class VariableType : BaseEntity<int>
{
    /// <summary>
    /// Variable type name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Variable type variables
    /// </summary>
    public ICollection<Variable> Variables { get; set; }
}