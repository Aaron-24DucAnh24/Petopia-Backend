using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.BackgroundJobs.Interfaces;
using PetAdoption.Business.Interfaces;

namespace PetAdoption.BackgroundJobs.Implementations
{
  public class CacheJobService : BaseJobService, ICacheJobService
  {
    private IBackgroundJobClient JobClient
    { get { return GetRequiredService<IBackgroundJobClient>(); } }

    public CacheJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    private async Task InitializeCacheDataTask()
    {
      // TODO: Execute services to init data here 
    }

    public void InitializeCacheData()
    {
      JobClient.Enqueue(() => InitializeCacheDataTask());
    }
  }
}