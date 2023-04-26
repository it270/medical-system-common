using System;
using It270.MedicalSystem.Common.Presentation.WebApi.Config;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Configuration;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Extensions;

public static class ClientInfoLoggerConfigurationExtensions
{
    public static LoggerConfiguration WithUserName<T>(this LoggerEnrichmentConfiguration enrich)
        where T : class, IHttpContextAccessor, new()
    {
        if (enrich == null)
            throw new ArgumentNullException(nameof(enrich));

        return enrich.With<UserEnricher<T>>();
    }
}