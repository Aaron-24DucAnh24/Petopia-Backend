using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Petopia.BackgroundJobs.Implementations;
using Petopia.BackgroundJobs.Interfaces;
using Petopia.Business.Constants;

namespace Petopia.BackgroundJobs.Extensions
{
  public static class ServiceExtension
  {
    public static void AddBackgroundServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddHangfire(config => config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(configuration.GetConnectionString(AppSettingKey.DB_CONNECTION_STRING)));
      services.AddHangfireServer();
      services.AddHostedService<InitJobsService>(); // important
      services.AddScoped<ICacheJobService, CacheJobService>();
      services.AddScoped<IEmailJobService, EmailJobService>();
      services.AddScoped<IElasticsearchJobService, ElasticsearchJobService>();
    }
  }
}