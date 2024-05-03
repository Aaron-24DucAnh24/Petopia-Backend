using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class AdvertisementConfiguration : IEntityTypeConfiguration<Advertisement>
  {
    public void Configure(EntityTypeBuilder<Advertisement> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Advertisement");
    }
  }
}