using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;

namespace Petopia.Data.Configurations
{
  public class PostConfiguration : IEntityTypeConfiguration<Post>
  {
    public void Configure(EntityTypeBuilder<Post> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Post");
      builder.Property(x => x.Like).HasDefaultValue(0);
      builder
        .HasMany(x => x.Images)
        .WithOne(x => x.Post)
        .HasForeignKey(x => x.PostId);
      builder
        .HasMany(x => x.Comments)
        .WithOne(x => x.Post)
        .HasForeignKey(x => x.PostId);
    }
  }
}



