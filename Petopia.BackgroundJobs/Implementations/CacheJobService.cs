using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Interfaces;
using Petopia.Business.Models.Location;
using Petopia.Data.Enums;

namespace Petopia.BackgroundJobs.Implementations
{
  public class CacheJobService : BaseJobService, ICacheJobService
  {
    public CacheJobService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public void InitCacheData()
    {
      BackgroundJob.Enqueue(() => InitCacheDataTask());
    }

    public async Task<Task> InitCacheDataTask()
    {
      await ServiceProvider.GetRequiredService<IPetService>().GetBreedsAsync(PetSpecies.Cat);
      await ServiceProvider.GetRequiredService<IPetService>().GetBreedsAsync(PetSpecies.Dog);
      await ServiceProvider.GetRequiredService<IPetService>().GetAvailableBreedsAsync(PetSpecies.Cat);
      await ServiceProvider.GetRequiredService<IPetService>().GetAvailableBreedsAsync(PetSpecies.Dog);
      await ServiceProvider.GetRequiredService<ILocationService>().GetLocation(new LocationRequestModel()
      {
        Level = 1,
      });
      return Task.CompletedTask;
    }
  }
}