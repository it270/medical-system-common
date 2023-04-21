using System.Net.Mime;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;

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

    /// <summary>
    /// Check if a file has the JPEG MIME type
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is a JPEG image. False otherwise</returns>
    public static bool IsImageJpeg(this IFormFile file)
    {
        return file?.ContentType == MediaTypeNames.Image.Jpeg;
    }

    /// <summary>
    /// Check if a file has a valid size
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if has a valid size. False otherwise</returns>
    public static bool HasValidSize(this IFormFile file)
    {
        return file?.Length <= GeneralConstants.FileSizeLimit;
    }
}