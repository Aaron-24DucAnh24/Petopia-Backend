using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Data.Configurations
{
  public class PetConfiguration : IEntityTypeConfiguration<Pet>
  {
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Pet");
      builder.Property(x => x.IsCreatedAt).HasDefaultValue(DateTimeOffset.Now);
      builder.Property(x => x.IsUpdatedAt).HasDefaultValue(DateTimeOffset.Now);
      builder
        .HasMany<Media>(x => x.Images)
        .WithOne(x => x.Pet)
        .HasForeignKey(x => x.PetId);
    }
  }
}