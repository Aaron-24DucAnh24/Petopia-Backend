using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Petopia.Data.Entities;

namespace Petopia.Data
{
  public static class DbContextSeeding
  {
    public static void AddDataSeeding(this ModelBuilder modelBuilder)
    {
      var admins = JsonSerializer.Deserialize<List<User>>(
        new StreamReader("../Petopia.Data/SeedingData/Admins.json").ReadToEnd()
      );

      if (admins != null)
      {
        foreach (var admin in admins)
        {
          admin.Id = Guid.NewGuid();
          var attributes = JsonSerializer.Deserialize<UserIndividualAttributes>(
            JsonSerializer.Serialize(admin.UserIndividualAttributes)
          );
          if (attributes == null)
          {
            break;
          }
          attributes.Id = admin.Id;
          admin.UserIndividualAttributes = null;
          modelBuilder.Entity<User>().HasData(admin);
          modelBuilder.Entity<UserIndividualAttributes>().HasData(attributes);
        }
      }
    }
  }
}