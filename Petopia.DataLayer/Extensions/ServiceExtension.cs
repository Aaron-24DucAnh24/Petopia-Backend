using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Petopia.Data;
using Petopia.DataLayer.Implementations;
using Petopia.DataLayer.Interfaces;

namespace Petopia.DataLayer.Extensions
{
  public static class ServiceExtension
  {
    public static void AddApplicationDbContext(
      this IServiceCollection services, 
      IConfiguration configuration, 
      string ConnectionStringName )
    {
      services.AddDbContext<ApplicationDbContext>(options => 
      {
        options.UseSqlServer(configuration.GetConnectionString(ConnectionStringName));
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
      }, ServiceLifetime.Scoped);
    }

    public static void AddDataLayerServices(this IServiceCollection services)
    {
      services.AddScoped<IUserDataLayer, UserDataLayer>();
      services.AddScoped<IUserConnectionDataLayer, UserConnectionDataLayer>();
      services.AddScoped<ISyncDataCollectionDataLayer, SyncDataCollectionDataLayer>();
      services.AddScoped<IUserIndividualAttributesDataLayer, UserIndividualAttributesDataLayer>();
      services.AddScoped<IUserOrganizationAttributesDataLayer, UserOrganizationAttributesDataLayer>();
    }
  }
}