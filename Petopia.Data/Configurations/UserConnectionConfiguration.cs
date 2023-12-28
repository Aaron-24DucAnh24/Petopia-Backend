using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
  {
    void IEntityTypeConfiguration<UserConnection>.Configure(EntityTypeBuilder<UserConnection> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UserConnection");
      builder.Property(x => x.AccessTokenExpirationDate).HasDefaultValue(DateTimeOffset.Now);
      builder.Property(x => x.RefreshTokenExpirationDate).HasDefaultValue(DateTimeOffset.Now);
    }
  }
}