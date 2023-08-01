using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.ExternalServices;
using It270.MedicalSystem.Common.Application.Core.Helpers.Storage;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace It270.MedicalSystem.Common.Infrastructure.Infrastructure.Services;

/// <summary>
/// S3 storage Service
/// </summary>
public class StorageService : IStorageService
{
    private readonly ILogger _logger;
    private readonly IAmazonS3 _client;
    private readonly string _containerName;

    /// <summary>
    /// Service constructor
    /// </summary>
    /// <param name="logger">Serilog logger</param>
    public StorageService(ILogger logger)
    {
        _logger = logger;
        _containerName = Environment.GetEnvironmentVariable("AWS_STORAGE_BUCKET");
        _client = InitClient();
    }

    /// <summary>
    /// Initialize AWS client
    /// </summary>
    /// <returns>AWS client</returns>
    private static AmazonS3Client InitClient()
    {
        var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        var secretkey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        var region = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS_STORAGE_REGION"));

        var config = new AmazonS3Config
        {
            RegionEndpoint = region,
            ServiceURL = Environment.GetEnvironmentVariable("AWS_STORAGE_URL"),
            UseHttp = true,
            ForcePathStyle = true,
            Timeout = new TimeSpan(0, 0, 10),
        };

        return new(accessKey, secretkey, config);
    }

    /// <summary>
    /// Check existing file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>True if the process is successful and file exists. False otherwise.</returns>
    public async Task<bool> CheckExistingFile(string fileName, CancellationToken ct = default)
    {
        try
        {
            var request = new GetObjectAttributesRequest()
            {
                BucketName = _containerName,
                Key = fileName,
            };
            var response = await _client.GetObjectAttributesAsync(request, ct);

            return response?.ObjectSize > 0;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Check existing file {@docName}", fileName);
        }

        return false;
    }

    /// <summary>
    /// Upload file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="file">Upload file data</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    public async Task<bool> UploadFile(string fileName, IFormFile file, CancellationToken ct = default)
    {
        try
        {
            using (var newMemoryStream = new MemoryStream())
            {
                file.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = fileName,
                    BucketName = _containerName,
                    ContentType = file.ContentType,
                };

                var fileTransferUtility = new TransferUtility(_client);

                await fileTransferUtility.UploadAsync(uploadRequest, ct);

                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Upload file");
        }

        return false;
    }

    /// <summary>
    /// Download file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>Downloaded file data. Null otherwise.</returns>
    public async Task<FileData> DownloadFile(string fileName, CancellationToken ct = default)
    {
        MemoryStream ms = null;
        var fileData = new FileData();

        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _containerName,
                Key = fileName
            };

            using (var response = await _client.GetObjectAsync(request, ct))
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    fileData.MimeType = response.Headers.ContentType;
                    using (ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms, ct);
                    }
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
            {
                _logger.Error("Error: Document {@docName} not found", fileName);
                return null;
            }

            fileData.File = ms.ToArray();
            return fileData;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Download file");
            return null;
        }
    }

    /// <summary>
    /// Delete file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    public async Task<bool> DeleteFile(string fileName, CancellationToken ct = default)
    {
        return await DeleteFile(fileName, null, ct);
    }

    /// <summary>
    /// Delete file
    /// </summary>
    /// <param name="fileName">File name</param>
    /// <param name="versionId">Version identifier</param>
    /// <param name="ct">Cancellation token (optional)</param>
    /// <returns>True if the process is successful. False otherwise.</returns>
    public async Task<bool> DeleteFile(string fileName, string versionId, CancellationToken ct = default)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _containerName,
                Key = fileName
            };

            if (!string.IsNullOrEmpty(versionId))
                request.VersionId = versionId;

            var response = await _client.DeleteObjectAsync(request, ct);

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error: Delete file");
        }

        return false;
    }
}