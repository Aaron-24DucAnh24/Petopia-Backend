using Microsoft.EntityFrameworkCore;
using Petopia.Data.Configurations;
using Petopia.Data.Entities;

#nullable disable

namespace Petopia.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfiguration(new UserConfiguration());
      builder.ApplyConfiguration(new UserConnectionConfiguration());
      builder.ApplyConfiguration(new UserIndividualAttributesConfiguration());
      builder.ApplyConfiguration(new UserOrganizationAttributesConfiguration());
      builder.ApplyConfiguration(new SyncDataCollectionConfiguration());
      builder.AddDataSeeding();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserConnection> UserConnections { get; set; }
    public DbSet<UserIndividualAttributes> UserIndividualAttributes { get; set; }
    public DbSet<UserOrganizationAttributes> UserOrganizationAttributes { get; set; }
    public DbSet<SyncDataCollection> SyncDataCollection { get; set; }
  }
}