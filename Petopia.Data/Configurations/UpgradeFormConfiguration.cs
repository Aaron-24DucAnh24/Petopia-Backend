using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Data.Configurations
{
  public class UpgradeFormConfiguration : IEntityTypeConfiguration<UpgradeForm>
  {
    public void Configure(EntityTypeBuilder<UpgradeForm> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UpgradeForm");
      builder.Property(x => x.Status).HasDefaultValue(UpgradeStatus.Pending);
    }
  }
}

