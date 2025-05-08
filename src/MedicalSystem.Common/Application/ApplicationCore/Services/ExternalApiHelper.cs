using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;
using It270.MedicalSystem.Common.Application.ApplicationCore.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace It270.MedicalManagement.EthicsCommittee.Application.ApplicationCore.Services.General
{
    public class ExternalApiHelper : IExternalApiHelper
    {
        private readonly TokenService _tokenService;

        public ExternalApiHelper(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        #region ValidateMicroservice
        /// <summary>
        /// ValidateExist
        /// </summary>
        /// <param name="microservice"></param>
        /// <param name="service"></param>
        /// <param name="data"></param>
        /// <returns>bool</returns>
        public async Task<bool> ValidateExist(string microservice, string service, string data)
        {
            HttpClient httpClient = new();
            string token = _tokenService.GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string urlThirdPartys = $"{microservice}/{service}/{data}";
            HttpResponseMessage response = await httpClient.GetAsync(urlThirdPartys);

            var errorStatusCodes = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.BadRequest,
                HttpStatusCode.GatewayTimeout,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.RequestTimeout,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.NotAcceptable,
                HttpStatusCode.Conflict,
                HttpStatusCode.PreconditionFailed,
                HttpStatusCode.NotFound 
            };

            if (errorStatusCodes.Contains(response.StatusCode))
            {
                return false;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region GetJsonFromMicroserviceAsync
        /// <summary>
        /// GetJsonFromMicroserviceAsync
        /// </summary>
        /// <param name="microservice"></param>
        /// <param name="service"></param>
        /// <param name="data"></param>
        /// <returns>Json response</returns>
        public async Task<string?> GetJsonFromMicroservice(string microservice, string service, string data)
        {
            HttpClient httpClient = new();
            string token = _tokenService.GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string url = $"{microservice}/{service}/{data}";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            var errorStatusCodes = new HashSet<HttpStatusCode>
            {
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.BadRequest,
                HttpStatusCode.GatewayTimeout,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.RequestTimeout,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.NotAcceptable,
                HttpStatusCode.Conflict,
                HttpStatusCode.PreconditionFailed,
                HttpStatusCode.NotFound
            };

            if (errorStatusCodes.Contains(response.StatusCode) || response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            string jsonContent = await response.Content.ReadAsStringAsync();
            return jsonContent;
        }
        #endregion


    }
}
