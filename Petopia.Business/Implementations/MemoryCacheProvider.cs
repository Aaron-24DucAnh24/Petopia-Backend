using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Common;

namespace Petopia.Business.Implementations
{
  public class MemoryCacheProvider : ICacheProvider
  {
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;

    public MemoryCacheProvider(
      IMemoryCache memoryCache,
      ILogger logger
    )
    {
      _cache = memoryCache;
      _logger = logger;
    }

    public T Set<T>(string key, T value, Double expiration)
    {
      var options = new CacheProviderOptions();
      options.AbsoluteExpiration = DateTimeOffset.Now.AddDays(expiration);
      _cache.Set(key, value, new MemoryCacheEntryOptions()
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
        return _cache.Get<T>(key);
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
      catch (Exception e)
      {
        _logger.LogWarning("{Message}", e.Message);
        return false;
      }
    }

    public async ValueTask<IEnumerable<T>?> GetOrSetAsync<T>(IQueryable<T> query, string key, TimeSpan? cacheDuration = null)
    {
      var options = new CacheProviderOptions();
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

    private T? Set<T>(string key, T value, CacheProviderOptions options)
    {
      try
      {
        _cache.Set(key, value, new MemoryCacheEntryOptions()
        {
          AbsoluteExpiration = options.AbsoluteExpiration,
          AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
          SlidingExpiration = options.SlidingExpiration
        });
        return value;
      }
      catch (Exception)
      {
        return default;
      }
    }
  }
}