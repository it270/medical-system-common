using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.ApplicationCore.Interfaces.Repositories;
using It270.MedicalSystem.Common.Application.Core.Constants;
using It270.MedicalSystem.Common.Application.Core.Helpers.General;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace It270.MedicalSystem.Common.Infrastructure.Data.Repositories;

/// <summary>
/// Cache database repository
/// Code based on https://stackoverflow.com/a/60385140
/// </summary>
public class CacheRepository : ICacheRepository
{
    private readonly IDistributedCache _cache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly string _instanceName;

    /// <summary>
    /// Default constructor
    /// </summary>
    public CacheRepository(IDistributedCache cache,
        IConnectionMultiplexer connectionMultiplexer,
        IConfiguration configuration)
    {
        _cache = cache;
        _connectionMultiplexer = connectionMultiplexer;

        var settings = configuration.Get<CustomConfig>();
        _instanceName = settings?.Project?.Name ?? LogConstants.EmptyProjectName;
    }

    /// <summary>
    /// Remove keys by pattern
    /// </summary>
    /// <param name="keyPattern">Key pattern</param>
    /// <param name="ct">Cancellation token</param>
    public async Task RemoveWithWildCardAsync(string keyPattern, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(keyPattern))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(keyPattern));

        // get all the keys and remove each one
        await foreach (var key in GetKeysAsync(keyPattern))
        {
            await _cache.RemoveAsync(key, ct);
        }
    }

    /// <summary>
    /// Get keys by pattern
    /// </summary>
    /// <param name="pattern">Key pattern</param>
    /// <returns>Keys list</returns>
    public async IAsyncEnumerable<string> GetKeysAsync(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(pattern));

        foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
        {
            var server = _connectionMultiplexer.GetServer(endpoint);
            await foreach (var key in server.KeysAsync(pattern: $"*{pattern}"))
            {
                yield return RemoveInstanceName(key.ToString());
            }
        }
    }

    /// <summary>
    /// Remove instance from Redis key
    /// </summary>
    /// <param name="key">Complete Redis key</param>
    /// <returns>Redis key without instance name</returns>
    private string RemoveInstanceName(string key)
    {
        return key.Replace(_instanceName, string.Empty);
    }
}