using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Aggregates.Booking;

namespace Infrastructure.Configurations;

public class PaymentRecordConfiguration : IEntityTypeConfiguration<PaymentRecord>
{
       public void Configure(EntityTypeBuilder<PaymentRecord> builder)
       {
              // Primary key
              builder.HasKey(p => p.PaymentId);

              // Properties
              builder.Property(p => p.AmountPaid)
                     .HasPrecision(18, 2)
                     .IsRequired();

              builder.Property(p => p.PaymentDate)
                     .IsRequired();

              builder.Property(p => p.PaymentMethod)
                     .HasMaxLength(100);

              // Relationship to Invoice (no navigation back)
              builder.HasOne<Invoice>()               // no navigation property in PaymentRecord
                     .WithMany(i => i.Payments)
                     .HasForeignKey(p => p.InvoiceId)
                     .OnDelete(DeleteBehavior.Cascade);
       }
}