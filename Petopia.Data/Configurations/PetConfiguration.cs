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
        .HasMany(x => x.Images)
        .WithOne(x => x.Pet)
        .HasForeignKey(x => x.PetId);
      builder
        .HasMany(x => x.AdoptionForms)
        .WithOne(x => x.Pet)
        .HasForeignKey(x => x.PetId)
        .OnDelete(DeleteBehavior.NoAction);
      builder
        .HasMany(x => x.Posts)
        .WithOne(x => x.Pet)
        .HasForeignKey(x => x.PetId)
        .OnDelete(DeleteBehavior.NoAction);
    }
  }
}