using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class UserIndividualAttributesConfiguration : IEntityTypeConfiguration<UserIndividualAttributes>
  {
    void IEntityTypeConfiguration<UserIndividualAttributes>.Configure(EntityTypeBuilder<UserIndividualAttributes> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UserIndividualAttributes");
    }
  }
}