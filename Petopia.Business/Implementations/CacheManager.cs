using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Petopia.Business.Constants;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Setting;
using StackExchange.Redis;

namespace Petopia.Business.Implementations
{
  public class CacheManager : ICacheManager
  {
    private ConnectionMultiplexer? _redis = null;
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

    private void CheckDistributedCacheEnable(
      IDistributedCache? distributedCache,
      RedisCacheSettingModel? redisCacheSetting
    )
    {
      if (distributedCache != null)
      {
        try
        {
          if (redisCacheSetting != null)
          {
            _redis = ConnectionMultiplexer.Connect(redisCacheSetting.ConnectionString);
            _distributedCacheEnabled = true;
            _redis.ConnectionFailed += (sender, arg) =>
            {
              _distributedCacheEnabled = false;
              _logger.LogError("Connection failed. Redis is disabled.");
            };
            _redis.InternalError += (sender, arg) =>
            {
              _distributedCacheEnabled = false;
              _logger.LogError("Internal error. Redis is disabled.");
            };
            _redis.ConnectionRestored += (sender, arg) =>
            {
              _distributedCacheEnabled = true;
              _logger.LogError("Connection restored. Redis is enabled.");
            };
          }
        }
        catch (Exception)
        {
          _distributedCacheEnabled = false;
        }
      }
    }

    public CacheManager(
      IServiceProvider serviceProvider,
      ILogger<CacheManager> logger,
      IConfiguration configuration
    )
    {
      _logger = logger;
      _serviceProvider = serviceProvider;
      CheckDistributedCacheEnable(
        serviceProvider.GetService<IDistributedCache>(),
        configuration.GetSection(AppSettingKey.REDIS_CACHE).Get<RedisCacheSettingModel>()
      );
    }

    public ICacheProvider Instance
    {
      get
      {
        if (_distributedCacheEnabled)
        {
          _logger.LogInformation("Access Redis cache");
          return DistributedCacheProvider;
        }
        _logger.LogInformation("Access Memory cache");
        return MemoryCacheProvider;
      }
    }
  }
}