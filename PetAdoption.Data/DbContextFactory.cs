using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PetAdoption.Data;

namespace TicketBooking.API.DBContext
{
    class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
    {

      DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
      
      IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

      optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}