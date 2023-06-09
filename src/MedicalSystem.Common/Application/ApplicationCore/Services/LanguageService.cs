using System;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;
using It270.MedicalSystem.Common.Application.ApplicationCore.Specifications;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Entities.MultiLanguage;
using Serilog;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services;

/// <summary>
/// Language service
/// </summary>
public class LanguageService : ILanguageService
{
    private readonly ILogger _logger;
    private readonly IReadRepository<StringTemplate> _stringTemplateRepository;

    /// <summary>
    /// Default constructor
    /// </summary>
    public LanguageService(ILogger logger,
        IReadRepository<StringTemplate> stringTemplateRepository
    )
    {
        _logger = logger;
        _stringTemplateRepository = stringTemplateRepository;
    }

    /// <summary>
    /// Get string from key and language
    /// </summary>
    /// <typeparam name="KeyEnum">String key enum</typeparam>
    /// <param name="key">String key</param>
    /// <param name="language">Language abbreviation</param>
    /// <returns>Translated key</returns>
    public async Task<string> GetString<KeyEnum>(KeyEnum key, string language)
    where KeyEnum : Enum
    {
        // Validate language
        if (string.IsNullOrEmpty(language) || language.Length != 2)
            language = GeneralConstants.DefaultLanguage;

        language = language.ToLower();

        // Database search 
        var specification = new StringTemplateSpec(key.ToString(), language);
        var dataEntity = await _stringTemplateRepository.FirstOrDefaultAsync(specification);

        // Validate string result
        if (dataEntity == null)
        {
            _logger.Error("Error: not found string '{@key}' in '{@language}' language", key, language);
            return GeneralConstants.DefaultString;
        }

        return dataEntity.Value;
    }
}