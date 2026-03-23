using Domain.Enums;
using Domain.Exceptions;
namespace Domain.Aggregates.BookingAggregate;

public class Invoice
{
    public int InvoiceId { get; private set; }
    public int BookingId { get; private set; }  // only ID, not full Booking
    public decimal AmountDue { get; private set; }
    public DateTime IssueDate { get; private set; }
    public DateTime? DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Unpaid;

    private readonly List<PaymentRecord> _payments = new();
    public IReadOnlyCollection<PaymentRecord> Payments => _payments.AsReadOnly();

    internal Invoice( decimal amountDue, DateTime issueDate)
    {
        AmountDue = amountDue;
        IssueDate = issueDate;
    }

    public void AddPayment(decimal amount, DateTime paymentDate, string? method = null)
    {
        var payment = new PaymentRecord(this, amount, paymentDate, method);
        _payments.Add(payment);

        if (_payments.Sum(p => p.AmountPaid) >= AmountDue)
            Status = InvoiceStatus.Paid;
        else
            Status = InvoiceStatus.Partial;
    }

    public void UpdateAmountDue(decimal newAmount)
    {
        if (newAmount < 0)
            throw new DomainException("Invoice amount cannot be negative");
        AmountDue = newAmount;

        if (_payments.Sum(p => p.AmountPaid) >= AmountDue)
            Status = InvoiceStatus.Paid;
        else if (_payments.Any())
            Status = InvoiceStatus.Partial;
        else
            Status = InvoiceStatus.Unpaid;
    }
}