using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petopia.Data.Entities;
using Petopia.Data.Enums;

namespace Petopia.Data.Configurations
{
    internal class AdoptionFormConfiguration : IEntityTypeConfiguration<AdoptionForm>
    {
        public void Configure(EntityTypeBuilder<AdoptionForm> builder)
        {
            builder.HasKey(x => x.AdoptionFormId);
            builder.ToTable("AdoptionForm");
            builder.Property(x => x.PetId).HasDefaultValue(string.Empty);
            builder.Property(x => x.UserId).HasDefaultValue(string.Empty);
            builder.Property(x => x.AdoptStatus).HasDefaultValue(AdoptStatus.Pending);
            builder.Property(x => x.CreatedAt).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.FName).HasDefaultValue(string.Empty);
            builder.Property(x => x.LName).HasDefaultValue(string.Empty);
            builder.Property(x => x.Age).HasDefaultValue(0);
            builder.Property(x => x.Email).HasDefaultValue(string.Empty);
            builder.Property(x => x.PhoneNum).HasDefaultValue(string.Empty);
            builder.Property(x => x.Adr).HasDefaultValue(string.Empty);
            builder.Property(x => x.AdrCity).HasDefaultValue(string.Empty);
            builder.Property(x => x.AdrDistrict).HasDefaultValue(string.Empty);
            builder.Property(x => x.IsPetOwner).HasDefaultValue(false);
            builder.Property(x => x.HouseType).HasDefaultValue(HouseType.House);
            builder.Property(x => x.TakePetDuration).HasDefaultValue(TakePetDuration.OneWeek);

/*            builder.HasOne(x => x.PetId).WithMany().HasForeignKey(x => x.PetId);
            builder.HasOne(x => x.UserId).WithMany().HasForeignKey(x => x.UserId);*/
        }
    }
}
