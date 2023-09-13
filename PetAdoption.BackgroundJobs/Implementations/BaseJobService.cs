using Microsoft.Extensions.DependencyInjection;

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