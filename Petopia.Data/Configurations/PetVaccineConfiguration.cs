using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class PetVaccineConfiguration : IEntityTypeConfiguration<PetVaccine>
  {
    void IEntityTypeConfiguration<PetVaccine>.Configure(EntityTypeBuilder<PetVaccine> builder)
    {
      builder.HasKey(x => new { x.PetId, x.VaccineId });
      builder.ToTable("PetVaccine");
      builder
        .HasOne(x =>  x.Pet)
        .WithMany(x => x.PetVaccines)
        .HasForeignKey(x => x.PetId);
      builder
        .HasOne(x =>  x.Vaccine)
        .WithMany(x => x.PetVaccines)
        .HasForeignKey(x => x.VaccineId);
    }
  }
}