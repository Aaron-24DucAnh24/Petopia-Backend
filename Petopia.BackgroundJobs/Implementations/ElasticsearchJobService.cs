using Hangfire;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Interfaces;

namespace Petopia.BackgroundJobs.Implementations
{
  public class ElasticsearchJobService : BaseJobService, IElasticsearchJobService
  {
    public ElasticsearchJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void InitSyncDataCollections()
    {
      JobClient.Enqueue(() => GetRequiredService<IElasticsearchService>().InitSyncDataCollectionsAsync());
    }

    public void SyncData()
    {
      JobClient.Enqueue(() => GetRequiredService<IElasticsearchService>().SyncDataAsync());
    }
  }
}