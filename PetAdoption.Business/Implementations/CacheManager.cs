using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetAdoption.Business.Interfaces;
using StackExchange.Redis;

namespace PetAdoption.Business.Implementations
{
  public class CacheManager : ICacheManager
  {
    private readonly ConnectionMultiplexer _redis;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private bool _distributedCacheEnabled = false;

    private ICacheProvider DistributedCacheProvider
    {
      get
      {
        return new DistributedCacheProvider(_serviceProvider.GetRequiredService<IDistributedCache>(), _logger);
      }
    }

    private ICacheProvider MemoryCacheProvider
    {
      get
      {
        return new MemoryCacheProvider(_serviceProvider.GetRequiredService<IMemoryCache>(), _logger);
      }
    }

    private void CheckDistributedCacheEnable()
    {

    }

    public CacheManager
    (
      IServiceProvider serviceProvider,
      ILogger<CacheManager> logger
    )
    {
      _logger = logger;
      _serviceProvider = serviceProvider;
    }

    public ICacheProvider Instance
    {
      get
      {
        return _distributedCacheEnabled ? DistributedCacheProvider : MemoryCacheProvider;
      }
    }
  }
}