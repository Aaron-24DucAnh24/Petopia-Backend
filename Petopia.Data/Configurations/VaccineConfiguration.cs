using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class VaccineConfiguration : IEntityTypeConfiguration<Vaccine>
  {
    void IEntityTypeConfiguration<Vaccine>.Configure(EntityTypeBuilder<Vaccine> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Vaccine");
    }
  }
}