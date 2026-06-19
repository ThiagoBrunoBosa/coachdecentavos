using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachDecentavos.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(32);
        builder.Property(x => x.MeetingUrl).HasMaxLength(2000);
        builder.Property(x => x.Notes).HasMaxLength(4000);
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ConsultingPackage).WithMany().HasForeignKey(x => x.ConsultingPackageId);
        builder.HasOne(x => x.AvailabilitySlot).WithMany().HasForeignKey(x => x.AvailabilitySlotId);
        builder.HasIndex(x => x.AvailabilitySlotId).IsUnique();
    }
}
