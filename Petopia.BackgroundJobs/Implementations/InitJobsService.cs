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
        _logger.LogInformation("Clearing old BackgroundJobs");
        scope.ServiceProvider.GetRequiredService<IClearOldJobsService>().Clear();
        _logger.LogInformation("Old BackgroundJobs cleared successfully");

        _logger.LogInformation("Start initializing BackgroundJobs");
        scope.ServiceProvider.GetRequiredService<ISearchEngineJobService>().InitSearchData();
        scope.ServiceProvider.GetRequiredService<ICacheJobService>().InitCacheData();
        _logger.LogInformation("Initializing BackgroundJobs successfully");
      }

      return Task.CompletedTask;
    }
  }
}