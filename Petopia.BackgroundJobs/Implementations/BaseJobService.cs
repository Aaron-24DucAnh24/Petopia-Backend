using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Petopia.BackgroundJobs.Implementations
{
  public class BaseJobService
  {
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IConfiguration Configuration;
    protected readonly IBackgroundJobClient BackgroundJob;
    protected readonly IRecurringJobManager RecurringJob;

    public BaseJobService(IServiceProvider serviceProvider)
    {
      ServiceProvider = serviceProvider;
      Configuration = serviceProvider.GetRequiredService<IConfiguration>();
      BackgroundJob = serviceProvider.GetRequiredService<IBackgroundJobClient>();
      RecurringJob = new RecurringJobManager();
    }
  }
}