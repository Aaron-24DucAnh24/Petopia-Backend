using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetAdoption.Data.Entities;

namespace PetAdoption.Data.Configurations
{
  public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
  {
    void IEntityTypeConfiguration<UserConnection>.Configure(EntityTypeBuilder<UserConnection> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UserConnection");
      builder.Property(x => x.AccessToken).HasMaxLength(2048).HasDefaultValue("");
      builder.Property(x => x.AccessTokenExpirationDate).HasDefaultValue(DateTimeOffset.Now.AddDays(-1));
      builder.Property(x => x.IsDeleted).HasDefaultValue(false);
    }
  }
}