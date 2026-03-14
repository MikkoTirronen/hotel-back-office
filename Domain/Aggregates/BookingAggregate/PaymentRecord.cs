using Domain.Enums;

namespace Domain.Aggregates.BookingAggregate;

public class PaymentRecord
{
    public int PaymentId { get; private set; }
    public Invoice Invoice { get; private set; } = null!;
    public decimal AmountPaid { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public string? PaymentMethod { get; private set; }

    internal PaymentRecord(Invoice invoice, decimal amount, DateTime paymentDate, string? method)
    {
        Invoice = invoice;
        AmountPaid = amount;
        PaymentDate = paymentDate;
        PaymentMethod = method;
    }
}