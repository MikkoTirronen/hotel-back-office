
namespace Application.Bookings.Queries.GetAvailableRooms;
using Domain.Abstractions.Repositories;
using Application.DTOs;
public class GetAvailableRoomsHandler
{
    private readonly IBookingRepository _bookingRepo;
    private readonly IRoomRepository _roomRepo;

    public GetAvailableRoomsHandler(IBookingRepository bookingRepo, IRoomRepository roomRepo)
    {
        _bookingRepo = bookingRepo;
        _roomRepo = roomRepo;
    }

    public async Task<IEnumerable<RoomDto>> Handle(GetAvailableRoomsQuery query)
    {
        var rooms = await _roomRepo.GetAllAsync();
        var bookings = await _bookingRepo.GetBookingsInDateRangeAsync(query.StartDate, query.EndDate);

        return rooms
            .Where(r => r.Active && (r.BaseCapacity + r.MaxExtraBeds) >= query.Guests &&
                        !bookings.Any(b => b.RoomId == r.RoomId))
            .Select(r => new RoomDto(r))
            .ToList();
    }
}