using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.Business.Implementations
{
  public class DistributedCacheProvider : ICacheProvider
  {
    private readonly IDistributedCache _cache;
    private readonly ILogger _logger;

    public DistributedCacheProvider(
      IDistributedCache memoryCache,
      ILogger logger
    )
    {
      _cache = memoryCache;
      _logger = logger;
    }

    public ValueTask<IEnumerable<T>?> GetOrSetAsync<T>(IQueryable<T> query, string key, TimeSpan? cacheDuration = null)
    {
      throw new NotImplementedException();
    }

    public bool Remove(string key)
    {
      throw new NotImplementedException();
    }
  }

}