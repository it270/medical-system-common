﻿using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Entities.Fhir;
using System.Text.Json;
using Serilog;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services
{
    public class FhirService : IFhairService
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly string _gatewayUrl = Environment.GetEnvironmentVariable("MS_GATEWAY_URL");

        public FhirService(ILogger logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        async Task<Patient> IFhairService.AddPatient(Patient patient, CancellationToken ct)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_gatewayUrl}/Patient/{patient.Id}", ct);
                var jsonStr = await response.Content.ReadAsStringAsync(ct);
                patient = JsonSerializer.Deserialize<Patient>(jsonStr, GeneralConstants.DefaultJsonDeserializerOpts);

                return patient;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "gateway connection error");
                return null;
            }
        }

        async Task<Patient> IFhairService.GetPatient(string ccPatient, CancellationToken ct)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_gatewayUrl}/Patient?identifier={ccPatient}", ct);
                var jsonStr = await response.Content.ReadAsStringAsync(ct);
                var patient = JsonSerializer.Deserialize<Patient>(jsonStr, GeneralConstants.DefaultJsonDeserializerOpts);

                return patient;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "gateway connection error");
                return null;
            }
        }
        async Task<Patient> IFhairService.GetPatientDoc(string ccPatient,string idTypeDocument, CancellationToken ct)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_gatewayUrl}/Patient?identifier={ccPatient}&identifier={idTypeDocument}", ct);
                var jsonStr = await response.Content.ReadAsStringAsync(ct);
                var patient = JsonSerializer.Deserialize<Patient>(jsonStr, GeneralConstants.DefaultJsonDeserializerOpts);

                return patient;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "gateway connection error");
                return null;
            }
        }
    }
}
