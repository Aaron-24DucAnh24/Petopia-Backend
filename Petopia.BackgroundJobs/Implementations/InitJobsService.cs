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
      using (IServiceScope scope = _serviceProvider.CreateScope())
      {
        scope.ServiceProvider.GetRequiredService<ICacheJobService>().InitCacheData();
        scope.ServiceProvider.GetRequiredService<IElasticsearchJobService>().InitSyncDataCollections();
      }
      _logger.LogInformation("Start initializing BackgroundJobs");
      return Task.CompletedTask;
    }
  }
}