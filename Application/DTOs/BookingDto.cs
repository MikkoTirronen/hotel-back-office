using Domain.Enums;
using Domain.Aggregates.Booking;

namespace Application.DTOs;

public class BookingDto
{
    public BookingDto(Booking booking)
    {
        BookingId = booking.BookingId;
        RoomId = booking.RoomId;
        CustomerId = booking.CustomerId;
        StartDate = booking.StartDate;
        EndDate = booking.EndDate;
        NumPersons = booking.NumPersons;
        TotalPrice = booking.TotalPrice;
        ExtraBedsCount = booking.ExtraBedsCount;
        Status = booking.Status;
    }
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public int CustomerId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumPersons { get; set; }
    public decimal TotalPrice { get; set; }
    public int? ExtraBedsCount { get; set; }
    public BookingStatus Status { get; set; }

    public RoomDto? Room { get; set; }
    public CustomerDto? Customer { get; set; }
    public InvoiceDto? Invoice { get; set; }
}