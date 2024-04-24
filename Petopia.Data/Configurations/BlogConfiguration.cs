using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class BlogConfiguration : IEntityTypeConfiguration<Blog>
  {
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Blog");
      builder.Property(x => x.View).HasDefaultValue(0);
      builder.Property(x => x.Like).HasDefaultValue(0);
      builder
        .HasMany<Comment>(x => x.Comments)
        .WithOne(x => x.Blog)
        .HasForeignKey(x => x.BlogId);
    }
  }
}