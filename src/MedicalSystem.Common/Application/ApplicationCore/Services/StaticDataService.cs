﻿using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services;

/// <summary>
/// Static data module service
/// </summary>
public class StaticDataService : IStaticDataService
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly string _connectionGeographic = Environment.GetEnvironmentVariable("MS_STATIC_DATA_URL");

    /// <summary>
    /// Constructor
    /// </summary>
    public StaticDataService(ILogger logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Validate geographic region
    /// </summary>
    /// <param name="geographicRegionId">Geographic region identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if entity exists. false otherwise</returns>
    public async Task<bool> ValidateGeographicRegion(int geographicRegionId, CancellationToken ct = default)
    {
        try
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync($"{_connectionGeographic}/GeographicRegion/{geographicRegionId}", ct);
                var jsonStr = await response.Content.ReadAsStringAsync(ct);

                return !string.IsNullOrEmpty(jsonStr) && response.IsSuccessStatusCode;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Static data connection error");
            return false;
        }
    }

    /// <summary>
    /// Get geographic regions
    /// </summary>
    /// <param name="geographicRegionIds">Geographic regions identifiers</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Geographic regions as a dictionary</returns>
    public async Task<Dictionary<string, string>> GetGeographicRegions(int[] geographicRegionIds, CancellationToken ct = default)
    {
        try
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                var idsParams = geographicRegionIds
                    .Select(id => $"ids={id}");
                var idsParamsStr = string.Join('&', idsParams);

                var responseLocation = await client.GetAsync($"{_connectionGeographic}/GeographicRegion/GetByIds?{idsParamsStr}", ct);
                var staticDataGroup = JsonSerializer.Deserialize<Dictionary<string, string>>(await responseLocation.Content.ReadAsStringAsync(ct), GeneralConstants.DefaultJsonDeserializerOpts);

                return staticDataGroup;
            }

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Static data connection error");
            return new();
        }
    }
}