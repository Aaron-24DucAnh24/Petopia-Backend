using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PetAdoption.Data
{
  class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
      var settingFileDir = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ?
        "appsettings.Development.json" : "appsettings.json";
      var configuration = new ConfigurationBuilder()
        .SetBasePath($"{Directory.GetCurrentDirectory()}/../PetAdoption.API")
        .AddJsonFile(settingFileDir)
        .Build();
      optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}