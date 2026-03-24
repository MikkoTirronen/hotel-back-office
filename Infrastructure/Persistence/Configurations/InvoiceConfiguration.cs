using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Aggregates.Booking;

namespace Infrastructure.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
       public void Configure(EntityTypeBuilder<Invoice> builder)
       {
              builder.HasKey(i => i.Id);

              builder.Property(i => i.AmountDue)
                     .HasColumnName("InvoiceAmountDue")
                     .HasPrecision(18, 2)
                     .IsRequired();

              builder.Property(i => i.IssueDate)
                     .HasColumnName("InvoiceDate")
                     .IsRequired();

              var navigation = builder.Metadata.FindNavigation(nameof(Invoice.Payments));
              navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);
       }
}