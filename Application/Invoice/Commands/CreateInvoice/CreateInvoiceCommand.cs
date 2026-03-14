
namespace Application.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommand
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }

    public CreateInvoiceCommand(int bookingId, decimal amount)
    {
        BookingId = bookingId;
        Amount = amount;
    }
}