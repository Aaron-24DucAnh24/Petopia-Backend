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
      builder.Property(x => x.Id).HasDefaultValue(Guid.NewGuid().ToString());
      builder.ToTable("User");
      builder.Property(x => x.Role).HasDefaultValue(UserRole.StandardUser);
      builder.Property(x => x.Image).HasDefaultValue(string.Empty);
      builder
        .HasOne<UserConnection>(x => x.UserConnection)
        .WithOne(x => x.User)
        .HasForeignKey<UserConnection>(x => x.Id);
    }
  }
}