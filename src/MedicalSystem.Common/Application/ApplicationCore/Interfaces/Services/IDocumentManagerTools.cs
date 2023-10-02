using System;
using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.Core.Helpers.DocumentManager;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;

/// <summary>
/// Document manager tools interface
/// </summary>
public interface IDocumentManagerTools
{
    /// <summary>
    /// Get template data from external microservice
    /// </summary>
    /// <param name="url">External service url</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>External template data</returns>
    Task<TemplateParamValueHelper> GetTemplateDataExternal(Uri url, CancellationToken ct = default);
}