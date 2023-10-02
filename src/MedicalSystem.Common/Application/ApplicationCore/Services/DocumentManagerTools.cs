using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Helpers.DocumentManager;
using Serilog;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services;

/// <summary>
/// Document manager tools
/// </summary>
public class DocumentManagerTools : IDocumentManagerTools
{
    private readonly ILogger _logger;

    /// <summary>
    /// Default constructor
    /// </summary>
    public DocumentManagerTools(ILogger logger)
    {
        _logger = logger;
    }

    #region Template functions

    /// <summary>
    /// Get template data from external microservice
    /// </summary>
    /// <param name="url">External service url</param>
    /// <param name="moduleName">External module name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>External template data</returns>
    public async Task<TemplateParamValueHelper> GetTemplateDataExternal(Uri url, string moduleName, CancellationToken ct = default)
    {
        TemplateParamValueHelper response = null;

        try
        {
            var client = new HttpClient();
            var httpResponse = await client.GetAsync(url, ct);

            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.Error($"Module connection: Response error: {{@httpResponse}}", new
                {
                    Module = moduleName,
                    httpResponse.StatusCode,
                    httpResponse.ReasonPhrase,
                    httpResponse.Content,
                });

                return null;
            }

            var httpResponseContent = await httpResponse.Content.ReadAsStringAsync();
            var jsonOpts = GeneralConstants.DefaultJsonDeserializerOpts;
            jsonOpts.Converters.Add(new JsonStringEnumConverter());
            response = JsonSerializer.Deserialize<TemplateParamValueHelper>(httpResponseContent, jsonOpts);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Module connection: Exception. Request data: {{@debugData}}", new
            {
                Module = moduleName,
                Path = url,
            });
        }

        return response;
    }

    #endregion
}