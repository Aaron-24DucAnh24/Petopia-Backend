using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class PetBreedConfiguration : IEntityTypeConfiguration<PetBreed>
  {
    public void Configure(EntityTypeBuilder<PetBreed> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PetBreed");
    }
  }
}