using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class ReportConfiguration : IEntityTypeConfiguration<Report>
  {
    public void Configure(EntityTypeBuilder<Report> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Report");
    }
  }
}

