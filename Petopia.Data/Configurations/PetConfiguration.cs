using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class PetConfiguration : IEntityTypeConfiguration<Pet>
  {
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Pet");
      builder
        .HasMany<Media>(x => x.Images)
        .WithOne(x => x.Pet)
        .HasForeignKey(x => x.PetId);
      builder
        .HasMany<AdoptionForm>(x => x.AdoptionForms)
        .WithOne(x => x.Pet)
        .HasForeignKey(x => x.PetId)
        .OnDelete(DeleteBehavior.NoAction);
		}
  }
}