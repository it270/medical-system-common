using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Entities.Fhir;
using System.Text.Json;
using Serilog;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;

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

        async Task<Patient> IFhairService.AddPatient(Patient patient, string bearer, CancellationToken ct)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
                var response = await client.GetAsync($"{_gatewayUrl}/Patient", ct);
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

        async Task<List<Patient>> IFhairService.GetPatient(string ccPatient, string bearer, CancellationToken ct)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",  bearer);

                var response = await client.GetAsync($"{_gatewayUrl}/Patient?identifier={ccPatient}", ct);
                var jsonStr = await response.Content.ReadAsStringAsync(ct);
                var patient = JsonSerializer.Deserialize<List<Patient>>(jsonStr, GeneralConstants.DefaultJsonDeserializerOpts);

                return patient;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "gateway connection error");
                return null;
            }
        }

        public async Task<string> GetPatientDoc(string ccPatient, string idTypeDocument,string bearer, CancellationToken ct = default)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

                var response = await client.GetAsync($"{_gatewayUrl}/Patient/Exist?identifierValue={ccPatient}&identifierCode=1%{idTypeDocument}", ct);
                var jsonStr = await response.Content.ReadAsStringAsync(ct);

                return jsonStr!=null ? jsonStr : response.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "gateway connection error");
                return null;
            }
        }
    }
}
