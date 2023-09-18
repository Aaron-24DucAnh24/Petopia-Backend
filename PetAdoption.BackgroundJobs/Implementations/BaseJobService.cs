using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace PetAdoption.BackgroundJobs.Implementations
{
  public class BaseJobService
  {
    private readonly IServiceProvider _serviceProvider;
    protected readonly IBackgroundJobClient JobClient;

    public BaseJobService(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
      JobClient = serviceProvider.GetRequiredService<IBackgroundJobClient>();
    }

    protected T GetRequiredService<T>() where T : notnull
    {
      return _serviceProvider.GetRequiredService<T>();
    }
  }
}