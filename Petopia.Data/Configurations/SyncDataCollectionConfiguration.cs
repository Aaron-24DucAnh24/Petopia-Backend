using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Data.Configurations
{
  public class SyncDataCollectionConfiguration : IEntityTypeConfiguration<SyncDataCollection>
  {
    public void Configure(EntityTypeBuilder<SyncDataCollection> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Status).HasDefaultValue(SyncDataStatus.Waiting);
      builder.Property(x => x.Action).HasDefaultValue(SyncDataAction.Index);
    }
  }
}