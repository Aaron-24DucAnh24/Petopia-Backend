using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.BackgroundJobs.Interfaces;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.BackgroundJobs.Implementations
{
  public class CacheJobService : BaseJobService, ICacheJobService
  {
    public CacheJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    // This method must be public to be invoked
    public async Task InitializeCacheDataTask()
    {
      // Call all necessary methods to init cache data here
    }

    public void InitializeCacheData()
    {
      JobClient.Enqueue(() => InitializeCacheDataTask());
    }
  }
}