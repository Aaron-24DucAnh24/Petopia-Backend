using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Petopia.BackgroundJobs.Interfaces;

namespace Petopia.BackgroundJobs.Implementations
{
  public class InitJobsService : BackgroundService
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InitJobsService> _logger;

    public InitJobsService(
      IServiceProvider serviceProvider,
      ILogger<InitJobsService> logger
    )
    {
      _serviceProvider = serviceProvider;
      _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation("Start initializing BackgroundJobs");
      using (IServiceScope scope = _serviceProvider.CreateScope())
      {
        //scope.ServiceProvider.GetRequiredService<IElasticsearchJobService>().InitSyncDataCollections();
        scope.ServiceProvider.GetRequiredService<ICacheJobService>().InitCacheData();
      }
      _logger.LogInformation("Initializing BackgroundJobs successfully");

      return Task.CompletedTask;
    }
  }
}