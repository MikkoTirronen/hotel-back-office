using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Aggregates.Room;

namespace Infrastructure.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
       public void Configure(EntityTypeBuilder<Room> builder)
       {
              builder.HasKey(r => r.RoomId);

              builder.Property(r => r.RoomId)
                     .ValueGeneratedOnAdd(); // auto-increment

              builder.Property(r => r.RoomNumber)
                     .IsRequired()
                     .HasMaxLength(10);

              builder.HasIndex(r => r.RoomNumber)
                     .IsUnique();

              builder.Property(r => r.PricePerNight)
                     .HasPrecision(10, 2)
                     .IsRequired();

              // OwnsMany mapping for Amenities stays the same
              builder.OwnsMany(r => r.Amenities, a =>
              {
                     a.ToTable("RoomAmenities");
                     a.WithOwner().HasForeignKey("RoomId");
                     a.Property(x => x.Value).HasColumnName("Amenity").IsRequired();
                     a.HasKey("RoomId", "Value");
              });
              builder.Metadata.FindNavigation(nameof(Room.Amenities))!
                     .SetPropertyAccessMode(PropertyAccessMode.Field);

       }
}