using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class UserOrganizationAttributesConfiguration : IEntityTypeConfiguration<UserOrganizationAttributes>
  {
    void IEntityTypeConfiguration<UserOrganizationAttributes>.Configure(EntityTypeBuilder<UserOrganizationAttributes> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UserOrganizationAttributes");
    }
  }
}