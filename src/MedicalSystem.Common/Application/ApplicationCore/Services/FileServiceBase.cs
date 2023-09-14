using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.ApplicationCore.Extensions;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Services;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using It270.MedicalSystem.Common.Application.Core.Helpers.Storage;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace It270.MedicalSystem.Common.Application.ApplicationCore.Services;

/// <summary>
/// Application file service
/// </summary>
public abstract class FileServiceBase : IFileServiceBase
{
    private readonly ILogger _logger;
    private readonly IStorageService _storageSystem;
    private readonly string _fileStoragePrefix;

    /// <summary>
    /// Default constructor
    /// </summary>
    public FileServiceBase(string prefix, ILogger logger, IStorageService storageSystem)
    {
        _fileStoragePrefix = prefix;
        _logger = logger;
        _storageSystem = storageSystem;
    }

    /// <summary>
    /// Upload file
    /// </summary>
    /// <param name="name">File name</param>
    /// <param name="file">File data</param>
    /// <param name="hasValidSize">Valid file size flag</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<CustomWebResponse> Upload(string name, IFormFile file, bool hasValidSize = true, CancellationToken ct = default)
    {
        // Validate file
        bool isValidFile = !file.IsEmpty() && hasValidSize;

        if (!isValidFile)
            return new CustomWebResponse(true)
            {
                Message = "Invalid file data",
            };

        var filePath = $"{_fileStoragePrefix}/{name}";

        bool processSuccessful = await _storageSystem.UploadFile(filePath, file, ct);

        if (processSuccessful)
        {
            _logger.Information("Uploaded file: {@id}", filePath);
            return new CustomWebResponse()
            {
                ResponseBody = new KeyValuePair<string, string>("id", filePath),
            };
        }
        else
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Upload file error",
            };
    }

    /// <summary>
    /// Download file
    /// </summary>
    /// <param name="name">File name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<FileData> Download(string name, CancellationToken ct = default)
    {
        var filePath = $"{_fileStoragePrefix}/{name}";
        var fileData = await _storageSystem.DownloadFile(filePath, ct);

        if (fileData?.File == null)
            return null;

        _logger.Information("Downloaded file: {@id}", name);
        return fileData;
    }

    /// <summary>
    /// Delete file
    /// </summary>
    /// <param name="name">File name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Process result</returns>
    public async Task<CustomWebResponse> Delete(string name, CancellationToken ct = default)
    {
        var filePath = $"{_fileStoragePrefix}/{name}";
        bool processSuccessful = await _storageSystem.DeleteFile(filePath, ct);

        if (processSuccessful)
        {
            _logger.Information("Deleted file: {@fileName}", filePath);
            return new CustomWebResponse();
        }
        else
            return new CustomWebResponse(true)
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Delete file error",
            };
    }

    /// <summary>
    /// Get file content as string
    /// </summary>
    /// <param name="file">File data</param>
    /// <returns>File content as string</returns>
    public string GetString(IFormFile file)
    {
        var result = new StringBuilder();

        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
                result.AppendLine(reader.ReadLine());
        }

        return result.ToString();
    }
}