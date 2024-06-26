using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices
{
    internal interface IFhairService
    {
        /// <summary>
        /// Get boolean value from external microservice
        /// </summary>
        /// <param name="url">External service url</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if sentence is applied. false otherwise</returns>
        Task<HttpResponseMessage> GetEntityFhir(string url, int status,CancellationToken ct = default);
    }
}
