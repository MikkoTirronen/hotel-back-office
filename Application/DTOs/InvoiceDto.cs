using Domain.Enums;

namespace Application.DTOs;

public class InvoiceDto
{
    public InvoiceDto(Domain.Aggregates.Booking.Invoice invoice)
    {
        Id = invoice.Id;
        BookingId = invoice.BookingId;
        AmountDue = invoice.AmountDue;
        IssueDate = invoice.IssueDate;
        Status = invoice.Status;
    }
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal AmountDue { get; set; }
    public DateTime IssueDate { get; set; }
    public InvoiceStatus Status { get; set; }
    //public BookingDto? Booking { get; set; }
}