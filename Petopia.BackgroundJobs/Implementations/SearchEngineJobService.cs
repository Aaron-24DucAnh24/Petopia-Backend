using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Interfaces;

namespace Petopia.BackgroundJobs.Implementations
{
  public class SearchEngineJobService : BaseJobService, ISearchEngineJobService
  {
    public SearchEngineJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void InitSearchData()
    {
      BackgroundJob.Enqueue(() => InitSearchDataTask());
    }

    public async Task<Task> InitSearchDataTask()
    {
      await ServiceProvider.GetRequiredService<ISearchEngineService>().SyncDataAsync(isClean: true);
      return Task.CompletedTask;
    }
  }
}