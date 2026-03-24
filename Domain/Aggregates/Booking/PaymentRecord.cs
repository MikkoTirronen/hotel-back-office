
namespace Domain.Aggregates.Booking;

public class PaymentRecord
{
    public int PaymentId { get; private set; }
    public int InvoiceId { get; private set; }
    public decimal AmountPaid { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public string? PaymentMethod { get; private set; }
    private PaymentRecord() { }
    internal PaymentRecord(decimal amount, DateTime paymentDate, string? method)
    {
        AmountPaid = amount;
        PaymentDate = paymentDate;
        PaymentMethod = method;
    }
}