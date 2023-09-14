using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.BackgroundJobs.Implementations;
using PetAdoption.BackgroundJobs.Interfaces;
using PetAdoption.Business.Constants;

namespace PetAdoption.BackgroundJobs.Extensions
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
      services.AddScoped<ICacheJobService, CacheJobService>();
      services.AddHostedService<InitJobsService>();
    }
  }
}