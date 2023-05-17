using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;

/// <summary>
/// General file extensions
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Check if a file is empty
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is empty. False otherwise</returns>
    public static bool IsEmpty(this IFormFile file)
    {
        return file == null || file.Length <= 0;
    }
}