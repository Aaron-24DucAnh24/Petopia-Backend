using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PetAdoption.Data;

namespace TicketBooking.API.DBContext
{
    class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
  {
    ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
    {

      DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
      
      optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=PetAdoption;UID=SA;PWD=TicketBooking.database.v1;TrustServerCertificate=true;");

      return new ApplicationDbContext(optionsBuilder.Options);
    }
  }
}