using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.Core.Entities
{
    public class AuthenticationResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
    public class ResponseLogin
    {
        public AuthenticationResult AuthenticationResult { get; set; }
    }
}
