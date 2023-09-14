using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PetAdoption.Business.Interfaces;
using PetAdoption.Business.Models;

namespace PetAdoption.Business.Implementations
{
  public class DistributedCacheProvider : ICacheProvider
  {
    private readonly IDistributedCache _cache;
    private readonly ILogger _logger;

    public DistributedCacheProvider(
      IDistributedCache distributedCache,
      ILogger logger
    )
    {
      _cache = distributedCache;
      _logger = logger;
    }

    private T? Get<T>(string key)
    {
      try
      {
        string? cacheValue = _cache.GetString(key);
        if(string.IsNullOrEmpty(cacheValue))
        {
          return default;
        }
        return JsonSerializer.Deserialize<T>(cacheValue);
      }
      catch (Exception e)
      {
        _logger.LogWarning("{Message}", e.Message);
        return default;
      }
    }

    private T? Set<T>(string key, T value, CacheProviderOptions options)
    {
      try
      {
        _cache.SetString(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions()
        {
          AbsoluteExpiration = options.AbsoluteExpiration,
          AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
          SlidingExpiration = options.SlidingExpiration
        });
        return value;
      }
      catch (Exception e)
      {
        _logger.LogWarning("{Message}", e.Message);
        return default;
      }
    }

    public async ValueTask<IEnumerable<T>?> GetOrSetAsync<T>(IQueryable<T> query, string key, TimeSpan? cacheDuration = null)
    {
      CacheProviderOptions options = new();
      if (cacheDuration.HasValue)
      {
        options.AbsoluteExpiration = DateTimeOffset.Now.Add(cacheDuration.Value);
      }

      var result = Get<IEnumerable<T>>(key);
      if (result.IsNullOrEmpty())
      {
        result = await query.ToListAsync();
        Set(key, result, options);
      }

      return result;
    }

    public bool Remove(string key)
    {
      try
      {
        _cache.Remove(key);
        return true;
      }
      catch (Exception e)
      {
        _logger.LogWarning("{Message}", e.Message);
        return false;
      }
    }
  }

}