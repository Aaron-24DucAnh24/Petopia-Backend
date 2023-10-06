using Hangfire;
using Petopia.BackgroundJobs.Interfaces;

namespace Petopia.BackgroundJobs.Implementations
{
  public class CacheJobService : BaseJobService, ICacheJobService
  {
    public CacheJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void InitializeCacheData()
    {
      JobClient.Enqueue(() => InitializeCacheDataTask());
    }

    public async Task InitializeCacheDataTask()
    {
      // Call all necessary methods to init cache data here
    }
  }
}