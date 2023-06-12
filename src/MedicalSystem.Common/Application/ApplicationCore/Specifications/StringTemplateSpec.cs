using Ardalis.Specification;
using It270.MedicalSystem.Common.Application.Core.Entities.MultiLanguage;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Specifications;

/// <summary>
/// String template specification
/// </summary>
public class StringTemplateSpec : GeneralSpecification<int, StringTemplate>
{
    /// <summary>
    /// Default constructor
    /// </summary>
    public StringTemplateSpec()
    { }

    /// <summary>
    /// Constructor for one element query
    /// </summary>
    /// <param name="id">Element identifier</param>
    public StringTemplateSpec(int id)
        : base(id)
    {
        Query.Where(e => e.Id == id);
    }

    /// <summary>
    /// Constructor for one element query
    /// </summary>
    /// <param name="key">String key</param>
    /// <param name="language">Language abbreviation</param>
    public StringTemplateSpec(string key, string language)
    {
        Query.Include(e => e.Language)
           .Where(e => e.KeyString.Name == key && e.Language.Name == language);
    }
}