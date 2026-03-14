using Domain.Enums;

namespace Application.DTOs;
public class InvoiceDto
{
    public InvoiceDto(Domain.Aggregates.BookingAggregate.Invoice invoice)
    {
        InvoiceId = invoice.InvoiceId;
        BookingId = invoice.Booking.BookingId;
        AmountDue = invoice.AmountDue;
        IssueDate = invoice.IssueDate;
        Status = invoice.Status;
    }
    public int InvoiceId { get; set; }
    public int BookingId { get; set; }
    public decimal AmountDue { get; set; }
    public DateTime IssueDate { get; set; }
    public InvoiceStatus Status { get; set; }
    //public BookingDto? Booking { get; set; }
}