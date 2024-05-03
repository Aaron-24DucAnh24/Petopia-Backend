using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
  {
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Payment");
    }
  }
}