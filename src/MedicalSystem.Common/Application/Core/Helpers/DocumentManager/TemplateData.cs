using System.Collections.Generic;
using static It270.MedicalSystem.Common.Application.Core.Enums.DocumentManager.TemplateEnums;

namespace It270.MedicalSystem.Common.Application.Core.Helpers.DocumentManager;
public class TemplateData
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
}