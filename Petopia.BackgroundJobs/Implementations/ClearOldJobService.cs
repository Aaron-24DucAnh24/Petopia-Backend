using Hangfire;
using Petopia.BackgroundJobs.Interfaces;

namespace Petopia.BackgroundJobs.Implementations
{
  public class ClearOldJobService : BaseJobService, IClearOldJobsService
  {
    public ClearOldJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void Clear()
    {
      var storage = JobStorage.Current;
      var monitoringApi = storage.GetMonitoringApi();

      foreach (var job in monitoringApi.ScheduledJobs(0, int.MaxValue))
      {
        BackgroundJob.Delete(job.Key);
      }

      foreach (var job in monitoringApi.FailedJobs(0, int.MaxValue))
      {
        BackgroundJob.Delete(job.Key);
      }
    }
  }
}