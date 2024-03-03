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
        .HasOne<UserConnection>(x => x.UserConnection)
        .WithOne(x => x.User)
        .HasForeignKey<UserConnection>(x => x.Id);
      builder
        .HasOne<UserIndividualAttributes>(x => x.UserIndividualAttributes)
        .WithOne(x => x.User)
        .HasForeignKey<UserIndividualAttributes>(x => x.Id);
      builder
        .HasOne<UserOrganizationAttributes>(x => x.UserOrganizationAttributes)
        .WithOne(x => x.User)
        .HasForeignKey<UserOrganizationAttributes>(x => x.Id);
      builder
        .HasMany<Pet>(x => x.Pets)
        .WithOne(x => x.Owner)
        .HasForeignKey(x => x.OwnerId);
    }
  }
}