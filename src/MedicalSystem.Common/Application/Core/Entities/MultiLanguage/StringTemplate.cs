using It270.MedicalSystem.Common.Application.Core.Interfaces;

namespace It270.MedicalSystem.Common.Application.Core.Entities.MultiLanguage;

/// <summary>
/// String Template Entity
/// </summary>
public class StringTemplate : BaseEntity<int>, IAggregateRoot
{
    /// <summary>
    /// String Template Value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// KeyString identifier 
    /// </summary>
    public int KeyStringId { get; set; }

    /// <summary>
    /// Language identifier 
    /// </summary>
    public int LanguageId { get; set; }

    /// <summary>
    /// String Template Language 
    /// </summary>
    public Language Languages { get; set; }

    /// <summary>
    /// String Template Key String
    /// </summary>
    public KeyString KeyStrings { get; set; }
}