using System.Net.Mail;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.General;

/// <summary>
/// Web view tools
/// </summary>
public interface IWebViewTools
{
    /// <summary>
    /// Cast Web view to string
    /// </summary>
    /// <param name="viewName">View name</param>
    /// <param name="model">View model data</param>
    /// <param name="isPartial">Partial view flag</param>
    /// <returns>Web view as HTML string</returns>
    Task<string> RenderViewToStringAsync(string viewName, object model, bool isPartial = false);

    /// <summary>
    /// Cast view to alternate view (Email data)
    /// </summary>
    /// <param name="viewName">View name</param>
    /// <param name="model">View model data</param>
    /// <param name="isPartial">Partial view flag</param>
    /// <returns>Email alternate view</returns>
    Task<AlternateView> CastViewToAlternateViewAsync(string viewName, object model, bool isPartial = false);
}