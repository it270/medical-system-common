using System.Collections.Generic;
using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Core.Entities.MultiLanguage;

/// <summary>
/// Key's string template
/// </summary>
public class KeyString : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Key Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// key StringTemplates
    /// </summary>
    public ICollection<StringTemplate> StringTemplates { get; set; }

}