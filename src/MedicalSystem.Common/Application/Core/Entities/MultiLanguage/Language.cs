using System.Collections.Generic;
using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Core.Entities.MultiLanguage;

/// <summary>
/// Laguage entity
/// </summary>
public class Language : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// Language Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Language StringTemplates
    /// </summary>
    public ICollection<StringTemplate> StringTemplates { get; set; }
}