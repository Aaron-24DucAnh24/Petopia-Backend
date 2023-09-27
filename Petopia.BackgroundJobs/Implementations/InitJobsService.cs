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
      using (var scope = _serviceProvider.CreateScope())
      {
        var cacheJobService = scope.ServiceProvider.GetRequiredService<ICacheJobService>();
        cacheJobService.InitializeCacheData();
      }
      _logger.LogInformation("Start initializing BackgroundJobs");
      return Task.CompletedTask;
    }
  }
}