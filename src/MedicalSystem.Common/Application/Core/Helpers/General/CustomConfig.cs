namespace It270.MedicalSystem.Common.Application.Core.Helpers.General;

/// <summary>
/// Custom project configuration (appsettings)
/// </summary>
public class CustomConfig
{
    public ProjectData Project { get; set; }
}

public class ProjectData
{
    public string Name { get; set; }
}