using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.BackgroundJobs.Implementations;
using PetAdoption.BackgroundJobs.Interfaces;

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
        .UseSqlServerStorage(configuration.GetConnectionString("database")));
      services.AddHangfireServer();
      services.AddScoped<ISampleJobService, SampleJobService>();
    }
  }
}