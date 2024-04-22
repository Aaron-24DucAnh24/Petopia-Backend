//using Hangfire;
//using Microsoft.Extensions.DependencyInjection;
//using Petopia.BackgroundJobs.Interfaces;
//using Petopia.Business.Interfaces;

//namespace Petopia.BackgroundJobs.Implementations
//{
//  public class ElasticsearchJobService : BaseJobService, IElasticsearchJobService
//  {
//    public ElasticsearchJobService(IServiceProvider serviceProvider) : base(serviceProvider)
//    {
//    }

//    public void InitSyncDataCollections()
//    {
//      BackgroundJob.Enqueue(() => ServiceProvider.GetRequiredService<IElasticsearchService>().InitSyncDataCollectionsAsync());
//    }

//    public void SyncData()
//    {
//      BackgroundJob.Enqueue(() => ServiceProvider.GetRequiredService<IElasticsearchService>().SyncDataAsync());
//    }
//  }
//}