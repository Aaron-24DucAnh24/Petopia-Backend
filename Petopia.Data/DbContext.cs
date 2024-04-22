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
      builder.ApplyConfiguration(new PetConfiguration());
      builder.ApplyConfiguration(new ProvinceConfiguration());
      builder.ApplyConfiguration(new DistrictConfiguration());
      builder.ApplyConfiguration(new WardConfiguration());
      builder.ApplyConfiguration(new AdoptionFormConfiguration());
      builder.ApplyConfiguration(new NotificationConfiguration());
      builder.ApplyConfiguration(new UpgradeFormConfiguration());
      builder.ApplyConfiguration(new PostConfiguration());
      builder.ApplyConfiguration(new BlogConfiguration());
      builder.ApplyConfiguration(new CommentConfiguration());
      builder.ApplyConfiguration(new LikeConfiguration());
		}

    public DbSet<User> Users { get; set; }
    public DbSet<UserConnection> UserConnections { get; set; }
    public DbSet<UserIndividualAttributes> UserIndividualAttributes { get; set; }
    public DbSet<UserOrganizationAttributes> UserOrganizationAttributes { get; set; }
    public DbSet<SyncDataCollection> SyncDataCollections { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Ward> Wards { get; set; }
    public DbSet<AdoptionForm> AdoptionForms { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UpgradeForm> UpgradeForms { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Comment> Likes { get; set; }
	}
}