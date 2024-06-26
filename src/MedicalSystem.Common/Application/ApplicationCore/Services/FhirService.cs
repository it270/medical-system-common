using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;
using It270.MedicalSystem.Common.Application.Core.Entities;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services
{
    public class FhirService : IFhairService
    {
        #region StaticProperties 
        public static string AccessToken { get; set; }
        public static string Refresh { get; set; }
        #endregion

        #region GetAccessToken
        /// <summary>
        /// Method <GetAccessToken> get Access Token
        /// </summary>
        /// <returns>AccessToken</returns>
        public static string GetAccessToken() => AccessToken;
        #endregion

        #region Login
        /// <summary>
        /// Method <c>Log</c> Login Cognito
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Response log</returns>
        public async Task<CustomWebResponse> Log(CancellationToken ct = default)
        {
            string url = Environment.GetEnvironmentVariable("AWS_COGNITO_URL_COGNITO");
            var requestObject = new
            {
                AuthFlow = "USER_PASSWORD_AUTH",
                ClientId = Environment.GetEnvironmentVariable("AWS_COGNITO_CLIENT_ID_RECORDS"),
                AuthParameters = new
                {
                    USERNAME = Environment.GetEnvironmentVariable("AWS_COGNITO_USERNAME"),
                    PASSWORD = Environment.GetEnvironmentVariable("GENERAL_ADMIN_PASSWORD")
                }
            };

            var response = await ResponseCognitoPostAsync(JsonConvert.SerializeObject(requestObject), ct, url);

            AccessToken = response.AuthenticationResult.AccessToken;
            Refresh = response.AuthenticationResult.RefreshToken;
            if (response.AuthenticationResult.RefreshToken != null && response.AuthenticationResult.AccessToken != null)
                return new CustomWebResponse { StatusCode = HttpStatusCode.Accepted };
            else
                return new CustomWebResponse { StatusCode = HttpStatusCode.InternalServerError };
        }
        #endregion

        #region RefreshToken
        /// <summary>
        /// Method <c>RefreshToken</c> Refresh Token Cognito
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CustomWebResponse> RefreshToken(CancellationToken ct = default)
        {
            string url = Environment.GetEnvironmentVariable("AWS_COGNITO_URL_COGNITO");

            var requestObject = new
            {
                AuthFlow = "REFRESH_TOKEN_AUTH",
                ClientId = Environment.GetEnvironmentVariable("AWS_COGNITO_CLIENT_ID_RECORDS"),
                AuthParameters = new
                {
                    REFRESH_TOKEN = Refresh,
                }
            };
            ResponseLogin response = await ResponseCognitoPostAsync(JsonConvert.SerializeObject(requestObject), ct, url);
            return new CustomWebResponse { ResponseBody = response };
        }
        #endregion

        #region ResponseCognitoPostAsync
        /// <summary>
        /// Method <c>ResponseAsync</c> Response Async
        /// </summary>
        /// <param name="jsonContent">request json</param>
        /// <param name="ct"> CancellationToken </param>
        /// <returns>Response cognito</returns>
        public static async Task<ResponseLogin> ResponseCognitoPostAsync(string jsonContent, CancellationToken ct, string url)
        {
            using HttpClient httpClient = new();
            StringContent content = new(jsonContent, Encoding.UTF8, "application/x-amz-json-1.1");
            content.Headers.Add("X-Amz-Target", "AWSCognitoIdentityProviderService.InitiateAuth");
            HttpResponseMessage response = await httpClient.PostAsync(url, content, ct);
            string responseBody = await response.Content.ReadAsStringAsync(ct);
            return JsonConvert.DeserializeObject<ResponseLogin>(responseBody);
        }
        #endregion



        #region Get
        /// <summary>
        /// GetEntity
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ct"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetEntityFhir(string url, CancellationToken ct = default, int status = 0)
        {
            var token = GetAccessToken();
            if (token == null) 
                await Log(ct);
            
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/fhir+json");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetAccessToken()}");
            var response = await httpClient.GetAsync(url, ct);

            if (response.StatusCode == HttpStatusCode.Unauthorized && status == 0)
            {
                await RefreshToken(ct);
                response = await GetEntityFhir(url, ct, 1);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized && status == 1)
            {
                await Log(ct);
                response = await GetEntityFhir(url, ct, 2);
            }

            return response;
        }
        #endregion


    }
}
