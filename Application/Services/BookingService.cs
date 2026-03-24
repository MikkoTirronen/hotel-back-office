using Application.DTOs;
using Domain.Abstractions.Repositories;
using Domain.Aggregates.Booking;

public class BookingService
{
    private readonly IBookingRepository _bookingRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly IRoomRepository _roomRepo;

    public BookingService(
        IBookingRepository bookingRepo,
        ICustomerRepository customerRepo,
        IRoomRepository roomRepo)
    {
        _bookingRepo = bookingRepo;
        _customerRepo = customerRepo;
        _roomRepo = roomRepo;
    }

    public async Task<int> CreateBookingAsync(BookingDto dto)
    {
        var room = await _roomRepo.GetByIdAsync(dto.RoomId)
            ?? throw new Exception("Room not found");

        // Optionally check customer exists
        var customerExists = await _customerRepo.GetByIdAsync(dto.CustomerId) != null;
        if (!customerExists) throw new Exception("Customer not found");

        var booking = new Booking(
            dto.RoomId,
            dto.CustomerId,
            dto.StartDate,
            dto.EndDate,
            dto.NumPersons,
            dto.ExtraBedsCount ?? 0
        );

        // Calculate total price from room rate
        booking.CalculateTotal(room.PricePerNight);

        // Persist the booking only
        await _bookingRepo.CreateAsync(booking);
        await _bookingRepo.SaveAsync();

        return booking.BookingId;
    }

    // --- Get booking details ---
    public async Task<Booking?> GetBookingByIdAsync(int bookingId)
    {
        return await _bookingRepo.GetBookingDetailsAsync(bookingId);
    }

    // --- Cancel a booking ---
    public async Task CancelBookingAsync(int bookingId)
    {
        var booking = await _bookingRepo.GetByIdAsync(bookingId)
                      ?? throw new Exception("Booking not found");

        booking.Cancel(); // uses domain logic to prevent double cancellation

        await _bookingRepo.UpdateAsync(booking);
        await _bookingRepo.SaveAsync();
    }

    // --- Update a booking ---
    public async Task UpdateBookingAsync(int bookingId, DateTime newStart, DateTime newEnd, int guests, int extraBeds = 0)
    {
        var booking = await _bookingRepo.GetByIdAsync(bookingId)
                      ?? throw new Exception("Booking not found");

        // Check room availability for the new dates
        var availableRooms = await _roomRepo.GetAvailableRoomsAsync(newStart, newEnd);
        if (!availableRooms.Any(r => r.RoomId == booking.RoomId))
        {
            throw new Exception("Room not available for the new dates");
        }

        // Update aggregate
        booking.UpdateDates(newStart, newEnd);
        booking.UpdateGuests(guests, extraBeds);

        // Recalculate total price (assuming room price hasn't changed)
        var room = await _roomRepo.GetByIdAsync(booking.RoomId)
                   ?? throw new Exception("Room not found");
        booking.CalculateTotal(room.PricePerNight);

        await _bookingRepo.UpdateAsync(booking);
        await _bookingRepo.SaveAsync();
    }
}