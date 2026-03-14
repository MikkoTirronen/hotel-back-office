using Application.DTOs;

namespace Application.Bookings.Commands.CreateBooking;

public class CreateBookingCommand
{
    public CreateBookingWithCustomerDto Dto { get; set; } = null!;
}