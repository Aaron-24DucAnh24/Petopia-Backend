using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetAdoption.Business.Constants;
using PetAdoption.Business.Interfaces;
using PetAdoption.Business.Models;
using StackExchange.Redis;

namespace PetAdoption.Business.Implementations
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
          if (distributedCache is RedisCache)
          {
            if (redisCacheSetting != null && !string.IsNullOrEmpty(redisCacheSetting.ConnectionString))
            {
              _redis = ConnectionMultiplexer.Connect(redisCacheSetting.ConnectionString);
              _distributedCacheEnabled = true;
              _redis.ConnectionFailed += (sender, args) =>
              {
                _distributedCacheEnabled = false;
                _logger.LogError("Connection failed, disabling redis...");
              };

              _redis.InternalError += (sender, args) =>
              {
                _distributedCacheEnabled = false;
                _logger.LogError("Connection failed, disabling redis...");
              };

              _redis.ConnectionRestored += (sender, args) =>
              {
                _distributedCacheEnabled = true;
                _logger.LogInformation("Connection restored, redis is back...");
              };
            }
          }
          else
          {
            _distributedCacheEnabled = true;
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
        return _distributedCacheEnabled ? DistributedCacheProvider : MemoryCacheProvider;
      }
    }
  }
}