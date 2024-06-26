using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;

namespace Petopia.Business.Implementations
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

    public T Set<T>(string key, T value, double expiration)
    {
      CacheProviderOptions options = new()
      {
        AbsoluteExpiration = DateTimeOffset.Now.AddDays(expiration)
      };
      _cache.SetString(key, JsonSerializer.Serialize(value), new DistributedCacheEntryOptions()
      {
        AbsoluteExpiration = options.AbsoluteExpiration,
        AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
        SlidingExpiration = options.SlidingExpiration
      });
      return value;
    }

    public T? Get<T>(string key)
    {
      try
      {
        string? cacheValue = _cache.GetString(key);
        if (string.IsNullOrEmpty(cacheValue))
        {
          return default;
        }
        return JsonSerializer.Deserialize<T>(cacheValue);
      }
      catch (Exception)
      {
        return default;
      }
    }

    public bool Remove(string key)
    {
      try
      {
        _cache.Remove(key);
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public async ValueTask<List<T>?> GetOrSetAsync<T>(IQueryable<T> query, string key, double days)
    {
      List<T>? result = Get<List<T>>(key);

      if (result.IsNullOrEmpty())
      {
        result = await query.ToListAsync();
        Set(key, result, days);
      }

      return result;
    }
  }
}