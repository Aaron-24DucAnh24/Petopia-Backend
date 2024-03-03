using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
  {
    public void Configure(EntityTypeBuilder<Province> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Province");
    }
  }

  public class DistrictConfiguration : IEntityTypeConfiguration<District>
  {
    public void Configure(EntityTypeBuilder<District> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("District");
    }
  }

  public class WardConfiguration : IEntityTypeConfiguration<Ward>
  {
    public void Configure(EntityTypeBuilder<Ward> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Ward");
    }
  }
}