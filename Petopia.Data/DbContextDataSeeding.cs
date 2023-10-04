using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Petopia.Data.Entities;

namespace Petopia.Data
{
  public static class DbContextSeeding
  {
    public static void AddDataSeeding(this ModelBuilder modelBuilder)
    {
      var UsersJson = new StreamReader("../Petopia.Data/SeedingData/User.json").ReadToEnd();
      var users = JsonSerializer.Deserialize<List<User>>(UsersJson);

      if(users != null){
        foreach(var user in users)
        {
          user.Id = Guid.NewGuid().ToString();
          modelBuilder.Entity<User>().HasData(user);
        }
      }
    }
  }
}