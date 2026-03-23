using Domain.Abstractions.Repositories;
using Application.DTOs;

namespace Application.Bookings.Queries.GetAvailableRooms;

public class GetAvailableRoomsHandler
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IRoomRepository _roomRepo;

    public GetAvailableRoomsHandler(IBookingRepository bookingRepo, IRoomRepository roomRepo)
    {
        _bookingRepo = bookingRepo;
        _roomRepo = roomRepo;
    }

    // Plain async method instead of IRequestHandler
    public async Task<IEnumerable<RoomDto>> HandleAsync(DateTime startDate, DateTime endDate, int guests)
    {
        // Load all rooms
        var rooms = await _roomRepo.GetAllAsync();

        // Load bookings in the given date range
        var bookings = await _bookingRepo.GetBookingsInDateRangeAsync(startDate, endDate);

        // Filter available rooms
        var availableRooms = rooms
            .Where(r => r.Active &&
                        (r.BaseCapacity + r.MaxExtraBeds) >= guests &&
                        !bookings.Any(b => b.RoomId == r.RoomId))
            .Select(r => new RoomDto(r))
            .ToList();

        return availableRooms;
    }
}