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
      BackgroundJob.Enqueue(() => InitCacheDataTask());
    }

    public Task InitCacheDataTask()
    {
      // TODO
      return Task.CompletedTask;
    }
  }
}