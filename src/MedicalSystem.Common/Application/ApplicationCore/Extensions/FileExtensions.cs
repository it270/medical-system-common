using System.Linq;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Microsoft.AspNetCore.Http;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;

/// <summary>
/// General file extensions
/// </summary>
public static class FileExtensions
{
    #region General

    /// <summary>
    /// Check if a file is empty
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is empty. False otherwise</returns>
    public static bool IsEmpty(this IFormFile file)
    {
        return file == null || file.Length <= 0;
    }

    #endregion

    #region Files

    /// <summary>
    /// Check if is a valid image
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidImage(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Image.Jpeg,
            MediaTypeNames.Image.Png,
            MediaTypeNames.Image.Svg,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid html
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidHtml(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Document.Html,
            MediaTypeNames.Document.Xhtml,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid archive file
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidArchive(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Archive.SevenZip,
            MediaTypeNames.Archive.Zip,
            MediaTypeNames.Archive.Tar,
            MediaTypeNames.Archive.Rar,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid data file
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidData(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Data.Csv,
            MediaTypeNames.Data.Json,
            MediaTypeNames.Data.Xml,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid document
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidDocument(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Document.MsWord,
            MediaTypeNames.Document.MsWordX,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid spreadsheet
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidSpreadsheet(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Spreadsheet.MsExcel,
            MediaTypeNames.Spreadsheet.MsExcelX,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid slide
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidSlide(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Slide.MsPowerpoint,
            MediaTypeNames.Slide.MsPowerpointX,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid audio format
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidAudio(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Audio.Mp3,
            MediaTypeNames.Audio.Ogg,
            MediaTypeNames.Audio.Wav,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid video format
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsValidVideo(this IFormFile file)
    {
        var validImageMimeTypes = new string[] {
            MediaTypeNames.Video.Mp4,
            MediaTypeNames.Video.Mpeg,
            MediaTypeNames.Video.Avi,
            MediaTypeNames.Video.Ogg,
        };
        return validImageMimeTypes.Contains(file?.ContentType);
    }

    /// <summary>
    /// Check if is a valid pdf file
    /// </summary>
    /// <param name="file">Input file</param>
    /// <returns>True if is valid. False otherwise</returns>
    public static bool IsPdf(this IFormFile file)
    {
        return file?.ContentType == MediaTypeNames.Document.Pdf;
    }

    #endregion
}