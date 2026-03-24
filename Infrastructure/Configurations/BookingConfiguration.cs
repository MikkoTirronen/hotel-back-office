
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Aggregates.BookingAggregate;
using Domain.Aggregates.CustomerAggregate;

namespace Infrastructure.Configurations;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> entity)
    {
        entity.HasKey(b => b.BookingId);

        entity.Property(b => b.RoomId).IsRequired();
        entity.Property(b => b.CustomerId).IsRequired();
        entity.Property(b => b.StartDate).IsRequired();
        entity.Property(b => b.EndDate).IsRequired();
        entity.Property(b => b.NumPersons).IsRequired();
        entity.Property(b => b.ExtraBedsCount).IsRequired();
        entity.Property(b => b.TotalPrice).IsRequired();

        entity.Property(b => b.Status)
            .HasConversion<string>()
            .IsRequired();

        entity.OwnsOne(typeof(Invoice), "_invoice", inv =>
        {
            inv.Property("Amount").HasColumnName("InvoiceAmount");
            inv.Property("IssueDate").HasColumnName("InvoiceDate");
        });

        entity.HasOne<Customer>()
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId);
    }
}