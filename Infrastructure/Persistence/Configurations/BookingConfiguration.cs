using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Aggregates.Booking;
using Domain.Aggregates.Customer;

namespace Infrastructure.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
       public void Configure(EntityTypeBuilder<Booking> builder)
       {
              // Primary key
              builder.HasKey(b => b.BookingId);

              // Properties
              builder.Property(b => b.RoomId).IsRequired();
              builder.Property(b => b.CustomerId).IsRequired();
              builder.Property(b => b.StartDate).IsRequired();
              builder.Property(b => b.EndDate).IsRequired();
              builder.Property(b => b.NumPersons).IsRequired();
              builder.Property(b => b.ExtraBedsCount).IsRequired();
              builder.Property(b => b.TotalPrice).HasPrecision(18, 2).IsRequired();

              // Enum conversion
              builder.Property(b => b.Status)
                     .HasConversion<string>()
                     .IsRequired();

              // Relationships
              builder.HasOne<Customer>()
                     .WithMany(c => c.Bookings)
                     .HasForeignKey(b => b.CustomerId);

              // One-to-one relationship with Invoice
              builder.HasOne(b => b.Invoice) // or use backing field _invoice if you have it
                     .WithOne()
                     .HasForeignKey<Invoice>("BookingId")  // foreign key on Invoice
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Cascade);
       }
}