using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Petopia.Data.Entities;

namespace Petopia.Data
{
  public static class DbContextSeeding
  {
    public static void AddDataSeeding(this ModelBuilder modelBuilder)
    {
      /*------------------------------- INIT ADMINS -------------------------------  */
      List<User>? admins = JsonSerializer.Deserialize<List<User>>(
        new StreamReader("../Petopia.Data/SeedingData/Admins.json").ReadToEnd()
      );

      if (admins != null)
      {
        foreach (var admin in admins)
        {
          admin.Id = Guid.NewGuid();
          UserIndividualAttributes? attributes = JsonSerializer.Deserialize<UserIndividualAttributes>(
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

      /*------------------------------- INIT EMAILS -------------------------------  */
      List<EmailTemplate>? emails = JsonSerializer.Deserialize<List<EmailTemplate>>(
        new StreamReader("../Petopia.Data/SeedingData/Emails.json").ReadToEnd()
      );

      if (emails != null)
      {
        foreach (var email in emails)
        {
          email.Id = Guid.NewGuid();
          modelBuilder.Entity<EmailTemplate>().HasData(email);
        }
      }
    }
  }
}