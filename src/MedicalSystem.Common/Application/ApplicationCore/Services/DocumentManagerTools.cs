using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Helpers.DocumentManager;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services;

/// <summary>
/// Document manager tools
/// </summary>
public class DocumentManagerTools : IDocumentManagerTools
{
    private readonly string _moduleName;
    private readonly ILogger _logger;

    /// <summary>
    /// Default constructor
    /// </summary>
    public DocumentManagerTools(ILogger logger,
        IConfiguration configuration)
    {
        var settings = configuration.Get<CustomConfig>();
        _moduleName = settings?.Project?.ModuleName ?? LogConstants.EmptyModuleName;
        _logger = logger;
    }

    #region Template functions

    /// <summary>
    /// Get template data from external microservice
    /// </summary>
    /// <param name="url">External service url</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>External template data</returns>
    public async Task<TemplateParamValueHelper> GetTemplateDataExternal(Uri url, CancellationToken ct = default)
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
                    Module = _moduleName,
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
                Module = _moduleName,
                Path = url,
            });
        }

        return response;
    }

    #endregion
}