using System.Collections.Generic;
using static It270.MedicalSystem.Common.Application.Core.Enums.DocumentManager.TemplateEnums;

namespace It270.MedicalSystem.Common.Application.Core.Helpers.DocumentManager;

/// <summary>
/// Helper for template parameter value
/// </summary>
public class TemplateParamValueHelper
{
    /// <summary>
    /// Parameter name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Parameter type
    /// </summary>
    public TemplateParamTypeEnum Type { get; set; }

    /// <summary>
    /// Parameter value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Parameter children
    /// </summary>
    public List<TemplateParamValueHelper> Items { get; set; }
}