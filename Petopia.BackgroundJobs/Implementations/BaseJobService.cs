using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Petopia.BackgroundJobs.Implementations
{
  public class BaseJobService
  {
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IBackgroundJobClient BackgroundJob;

    public BaseJobService(IServiceProvider serviceProvider)
    {
      ServiceProvider = serviceProvider;
      BackgroundJob = serviceProvider.GetRequiredService<IBackgroundJobClient>();
    }
  }
}