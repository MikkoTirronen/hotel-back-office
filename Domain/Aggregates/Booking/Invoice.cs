using Domain.Enums;
using Domain.Exceptions;
namespace Domain.Aggregates.Booking;

public class Invoice
{
    private readonly List<PaymentRecord> _payments = new();
    public IReadOnlyCollection<PaymentRecord> Payments => _payments.AsReadOnly();
    public int Id { get; private set; }
    public int BookingId { get; private set; }
    public decimal AmountDue { get; private set; }
    public DateTime IssueDate { get; private set; }
    public DateTime? DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Unpaid;

    internal Invoice(decimal amountDue, DateTime issueDate)
    {
        AmountDue = amountDue;
        IssueDate = issueDate;
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
    public void AddPayment(decimal amount, DateTime paymentDate, string? method = null)
    {
        if (amount <= 0) throw new InvalidOperationException("Payment must be positive.");
        if (amount > AmountDue) throw new InvalidOperationException("Payment cannot exceed amount due.");

        var payment = new PaymentRecord(amount, paymentDate, method);
        _payments.Add(payment);

        AmountDue -= amount;
    }
}