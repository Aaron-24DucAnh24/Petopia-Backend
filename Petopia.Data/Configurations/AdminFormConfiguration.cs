using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Data.Configurations
{
  public class AdminFormConfiguration : IEntityTypeConfiguration<AdminForm>
  {
    public void Configure(EntityTypeBuilder<AdminForm> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("AdminForm");
      builder.Property(x => x.Status).HasDefaultValue(AdminFormStatus.Pending);
    }
  }
}

