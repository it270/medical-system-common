using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services
{
    public interface IExternalApiHelper
    {
        Task<bool> ValidateExist(string microservice, string service, string data);
        Task<string> GetJsonFromMicroservice(string microservice, string service, string data);
    }
}
