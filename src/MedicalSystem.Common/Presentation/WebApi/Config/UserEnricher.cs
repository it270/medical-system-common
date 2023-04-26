using It270.MedicalSystem.Common.Presentation.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Serilog user name enricher
/// </summary>
public class UserEnricher<T> : ILogEventEnricher
where T : class, IHttpContextAccessor, new()
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CLIENT_USER_PROPERTY_NAME = "UserName";
    private const string CLIENT_USER_ITEM_KEY = "Serilog_UserName";
    private const string USER_ANONYMOUS = "anonymous";

    public UserEnricher() : this(new T())
    { }

    public UserEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
            return;

        if (httpContext.Items[CLIENT_USER_ITEM_KEY] is LogEventProperty logEventProperty)
        {
            logEvent.AddPropertyIfAbsent(logEventProperty);
            return;
        }

        string userName = httpContext?.GetUserName() ?? USER_ANONYMOUS;

        var userNameProperty = new LogEventProperty(CLIENT_USER_PROPERTY_NAME, new ScalarValue(userName));
        httpContext.Items.Add(CLIENT_USER_ITEM_KEY, userNameProperty);

        logEvent.AddPropertyIfAbsent(userNameProperty);
    }
}