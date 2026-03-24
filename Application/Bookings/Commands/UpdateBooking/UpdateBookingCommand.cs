using Domain.Aggregates.Booking;
using Domain.Enums;
using Application.DTOs;
namespace Application.Bookings.Commands.UpdateBooking;

public class UpdateBookingCommand
{
    public int BookingId { get; set; }
    public int? RoomId { get; set; }
    public int CustomerId { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? NumPersons { get; set; }
    public decimal TotalPrice { get; set; }
    public int? ExtraBedsCount { get; set; }
    public BookingStatus? Status { get; set; }
}