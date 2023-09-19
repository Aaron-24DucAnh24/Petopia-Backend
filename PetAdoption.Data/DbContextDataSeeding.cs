using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PetAdoption.Data.Entities;

namespace PetAdoption.Data
{
  public static class DbContextSeeding
  {
    public static void AddDataSeeding(this ModelBuilder modelBuilder)
    {
      var UserReader = new StreamReader("../PetAdoption.Data/SeedingData/User.json");
      var UserConnectionReader = new StreamReader("../PetAdoption.Data/SeedingData/UserConnection.json");

      string UserJson = UserReader.ReadToEnd();
      string UserConnectionJson = UserConnectionReader.ReadToEnd();

      var users = JsonSerializer.Deserialize<List<User>>(UserJson);
      var userConnections = JsonSerializer.Deserialize<List<UserConnection>>(UserConnectionJson);

      if(users != null)
        foreach(var user in users)
          modelBuilder.Entity<User>().HasData(user);

      if(userConnections != null)
        foreach(var userConnection in userConnections)
          modelBuilder.Entity<UserConnection>().HasData(userConnection);
    }
  }
}