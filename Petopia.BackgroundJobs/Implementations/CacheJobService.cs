using Hangfire;
using Petopia.BackgroundJobs.Interfaces;

namespace Petopia.BackgroundJobs.Implementations
{
  public class CacheJobService : BaseJobService, ICacheJobService
  {
    public CacheJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void InitCacheData()
    {
      JobClient.Enqueue(() => InitCacheDataTask());
    }

    public Task InitCacheDataTask()
    {
      // Call all necessary methods to init cache data here
      return Task.CompletedTask;
    }
  }
}