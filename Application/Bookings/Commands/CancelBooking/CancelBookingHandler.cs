using Domain.Abstractions.Repositories;
namespace Application.Bookings.Commands.CancelBooking;

public class CancelBookingHandler
{
    private readonly IBookingRepository _bookingRepository;

    public CancelBookingHandler(IBookingRepository bookingRepo)
    {
        _bookingRepository = bookingRepo;
    }

    public async Task Handle(CancelBookingCommand command)
    {
        var booking = await _bookingRepository.GetByIdAsync(command.BookingId)
                     ?? throw new ApplicationException("Booking not found");

        booking.Cancel();

        await _bookingRepository.UpdateAsync(booking);
    }
}