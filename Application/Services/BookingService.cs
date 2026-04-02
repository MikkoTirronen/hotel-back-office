using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Abstractions.Repositories;
using Domain.Aggregates.Booking;
using Domain.Exceptions;


public class BookingService : IBookingService
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

    // public async Task<int> CreateBookingAsync(BookingDto dto, CancellationToken ct)
    // {
    //     var room = await _roomRepo.GetByIdAsync(dto.RoomId, ct)
    //         ?? throw new Exception("Room not found");

    //     // Optionally check customer exists
    //     var customerExists = await _customerRepo.GetByIdAsync(dto.CustomerId, ct) != null;
    //     if (!customerExists) throw new Exception("Customer not found");

    //     var booking = Booking.Create(
    //         dto.RoomId,
    //         dto.CustomerId,
    //         dto.StartDate,
    //         dto.EndDate,
    //         dto.NumPersons,
    //         dto.ExtraBedsCount ?? 0
    //     );

    //     booking.CalculateTotal(room.PricePerNight);

    //     await _bookingRepo.CreateAsync(booking);
    //     await _bookingRepo.SaveAsync();

    //     return booking.BookingId;
    // }
    public async Task<Result<int>> CreateBookingAsync(BookingDto dto, CancellationToken ct)
    {
        // 1. Check room exists
        var room = await _roomRepo.GetByIdAsync(dto.RoomId, ct);
        if (room == null)
            return Result<int>.Fail("Room not found");

        // 2. Check customer exists
        var customer = await _customerRepo.GetByIdAsync(dto.CustomerId, ct);
        if (customer == null)
            return Result<int>.Fail("Customer not found");

        // 3. Create booking aggregate
        Booking booking;
        try
        {
            booking = Booking.Create(
                dto.RoomId,
                dto.CustomerId,
                dto.StartDate,
                dto.EndDate,
                dto.NumPersons,
                dto.ExtraBedsCount ?? 0
            );
        }
        catch (DomainException ex)
        {
            return Result<int>.Fail(ex.Message);
        }

        // 4. Calculate total price
        booking.CalculateTotal(room.PricePerNight);

        // 5. Persist booking
        await _bookingRepo.CreateAsync(booking);
        await _bookingRepo.SaveAsync();

        // 6. Return booking ID as success
        return Result<int>.Success(booking.BookingId);
    }
    public async Task<Booking?> GetBookingByIdAsync(int bookingId, CancellationToken ct)
    {
        return await _bookingRepo.GetBookingDetailsAsync(bookingId, ct);
    }

    public async Task CancelBookingAsync(int bookingId, CancellationToken ct)
    {
        var booking = await _bookingRepo.GetByIdAsync(bookingId)
                      ?? throw new Exception("Booking not found");

        booking.Cancel(); // uses domain logic to prevent double cancellation

        await _bookingRepo.UpdateAsync(booking);
        await _bookingRepo.SaveAsync();
    }

    public async Task UpdateBookingAsync(int bookingId, DateTime newStart, DateTime newEnd, int guests, int extraBeds = 0, CancellationToken ct = default)
    {
        var booking = await _bookingRepo.GetByIdAsync(bookingId)
                      ?? throw new Exception("Booking not found");

        // Check room availability for the new dates
        var availableRooms = await _roomRepo.GetAvailableRoomsAsync(newStart, newEnd, ct);
        if (!availableRooms.Any(r => r.RoomId == booking.RoomId))
        {
            throw new Exception("Room not available for the new dates");
        }

        // Update aggregate
        booking.UpdateDates(newStart, newEnd);
        booking.UpdateGuests(guests, extraBeds);

        // Recalculate total price (assuming room price hasn't changed)
        var room = await _roomRepo.GetByIdAsync(booking.RoomId, ct)
                   ?? throw new Exception("Room not found");
        booking.CalculateTotal(room.PricePerNight);

        await _bookingRepo.UpdateAsync(booking, ct);
        await _bookingRepo.SaveAsync(ct);
    }
}