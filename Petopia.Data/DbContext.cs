using Microsoft.EntityFrameworkCore;
using Petopia.Data.Configurations;
using Petopia.Data.Entities;

namespace Petopia.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfiguration(new UserConfiguration());
      builder.ApplyConfiguration(new UserConnectionConfiguration());

      builder.AddDataSeeding();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserConnection> UserConnections { get; set; }
  }
}