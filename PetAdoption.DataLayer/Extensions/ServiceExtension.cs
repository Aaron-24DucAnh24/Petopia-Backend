using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetAdoption.Data;
using PetAdoption.DataLayer.Implementations;
using PetAdoption.DataLayer.Interfaces;

namespace PetAdoption.DataLayer.Extensions
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
    }
  }
}