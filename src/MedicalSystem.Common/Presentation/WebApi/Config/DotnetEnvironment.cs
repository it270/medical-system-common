using System;
using System.IO;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Config;

/// <summary>
/// Custom dotnet enviromnent variables class
/// </summary>
public static class DotnetEnvironment
{
    /// <summary>
    /// Load environment variables from a file
    /// </summary>
    /// <param name="filePath">Variables file path</param>
    public static void Load(string filePath)
    {
        // Code based in https://dusted.codes/dotenv-in-dotnet

        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}