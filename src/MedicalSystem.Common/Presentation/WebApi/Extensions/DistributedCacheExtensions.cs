using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using It270.MedicalSystem.Common.Application.Core.Constants;
using Microsoft.Extensions.Caching.Distributed;

namespace It270.MedicalSystem.Common.Presentation.WebApi.Extensions;

/// <summary>
/// Distributed cache extensions
/// Original from: https://code-maze.com/aspnetcore-distributed-caching/
/// </summary>
public static class DistributedCacheExtensions
{
    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Set cache value (with default options)
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="cache">Distributed cache of serialized values</param>
    /// <param name="key">Cache key</param>
    /// <param name="value">Cache value</param>
    public static Task SetAsyncDefault<T>(this IDistributedCache cache, string key, T value)
    {
        var defaultOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(CacheConstants.ConfSlidExp))
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(CacheConstants.ConfAbsExp));

        return SetAsync(cache, key, value, defaultOptions);
    }

    /// <summary>
    /// Set cache value (without options)
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="cache">Distributed cache of serialized values</param>
    /// <param name="key">Cache key</param>
    /// <param name="value">Cache value</param>
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value)
    {
        return SetAsync(cache, key, value, new DistributedCacheEntryOptions());
    }

    /// <summary>
    /// Set cache value
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="cache">Distributed cache of serialized values</param>
    /// <param name="key">Cache key</param>
    /// <param name="value">Cache value</param>
    /// <param name="options">Cache entry options</param>
    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _serializerOptions));
        return cache.SetAsync(key, bytes, options);
    }

    /// <summary>
    /// Try get cache value
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="cache">Distributed cache of serialized values</param>
    /// <param name="key">Cache key</param>
    /// <param name="value">Cache value</param>
    /// <returns>True if value exists. False otherwise</returns>
    public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T value)
    {
        var val = cache.Get(key);
        value = default;

        if (val == null)
            return false;

        value = JsonSerializer.Deserialize<T>(val, _serializerOptions);
        return true;
    }
}