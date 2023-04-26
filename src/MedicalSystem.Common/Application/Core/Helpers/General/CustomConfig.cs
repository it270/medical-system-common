namespace It270.MedicalSystem.Common.Application.Core.Helpers.General;

/// <summary>
/// Custom project configuration (appsettings)
/// </summary>
public class CustomConfig
{
    /// <summary>
    /// General project data
    /// </summary>
    public ProjectData Project { get; set; }
}

/// <summary>
/// Project data
/// </summary>
public class ProjectData
{
    /// <summary>
    /// Project name
    /// </summary>
    public string Name { get; set; }
}