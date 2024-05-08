using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Data.Configurations
{
  public class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("User");
      builder.Property(x => x.Role).HasDefaultValue(UserRole.StandardUser);
      builder.Property(x => x.Image).HasDefaultValue(string.Empty);
      builder.Property(x => x.Address).HasDefaultValue(string.Empty);
      builder
        .HasOne(x => x.UserConnection)
        .WithOne(x => x.User)
        .HasForeignKey<UserConnection>(x => x.Id);
      builder
        .HasOne(x => x.UserIndividualAttributes)
        .WithOne(x => x.User)
        .HasForeignKey<UserIndividualAttributes>(x => x.Id);
      builder
        .HasOne(x => x.UserOrganizationAttributes)
        .WithOne(x => x.User)
        .HasForeignKey<UserOrganizationAttributes>(x => x.Id);
      builder
        .HasMany(x => x.Pets)
        .WithOne(x => x.Owner)
        .HasForeignKey(x => x.OwnerId);
      builder
        .HasMany(x => x.AdoptionForms)
        .WithOne(x => x.Adopter)
        .HasForeignKey(x => x.AdopterId)
        .OnDelete(DeleteBehavior.NoAction);
      builder
        .HasMany(x => x.Notifications)
        .WithOne(x => x.User)
        .HasForeignKey(x => x.UserId);
      builder
        .HasOne(x => x.UpgradeForm)
        .WithOne(x => x.User)
        .HasForeignKey<UpgradeForm>(x => x.UserId);
      builder
        .HasMany(x => x.Blogs)
        .WithOne(x => x.User)
        .HasForeignKey(x => x.UserId);
    }
  }
}