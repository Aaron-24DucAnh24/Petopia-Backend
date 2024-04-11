using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Data.Configurations
{
	public class AdoptionFormConfiguration : IEntityTypeConfiguration<AdoptionForm>
	{
		public void Configure(EntityTypeBuilder<AdoptionForm> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("AdoptionForm");
			builder.Property(x => x.Status).HasDefaultValue(AdoptStatus.Pending);
			builder.Property(x => x.IsSeen).HasDefaultValue(false);
			builder.Property(x => x.IsSeenByAdmin).HasDefaultValue(false);
			builder.Property(x => x.IsOwnerBefore).HasDefaultValue(false);
			builder.Property(x => x.HouseType).HasDefaultValue(AdoptHouseType.House);
			builder.Property(x => x.DelayDuration).HasDefaultValue(AdoptDelayDuration.OneWeek);
		}
	}
}

