using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PetAdoption.BackgroundJobs.Implementations
{
  public class BaseJobService
  {
    protected readonly IServiceProvider ServiceProvider;

    public BaseJobService(IServiceProvider serviceProvider)
    {
      ServiceProvider = serviceProvider;
    }

    protected T GetRequiredService<T>() where T : notnull
    {
      return ServiceProvider.GetRequiredService<T>();
    }   
  }
}