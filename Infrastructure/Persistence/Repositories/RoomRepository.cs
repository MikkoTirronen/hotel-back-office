using Domain.Abstractions.Repositories;
using Domain.Aggregates.Room;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext _context;

    public RoomRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Room>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Rooms
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Room?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Rooms
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RoomId == id, ct);
    }

    public async Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default)
    {
        // Rooms that do NOT have a booking overlapping with the given date range
        return await _context.Rooms
            .Where(r => !_context.Bookings
                .Any(b => b.RoomId == r.RoomId &&
                          b.Status != Domain.Enums.BookingStatus.Canceled &&
                          b.StartDate < endDate &&
                          b.EndDate > startDate))
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task CreateAsync(Room entity, CancellationToken ct = default)
    {
        await _context.Rooms.AddAsync(entity, ct);
    }

    public Task UpdateAsync(Room entity, CancellationToken ct = default)
    {
        _context.Rooms.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Room entity, CancellationToken ct = default)
    {
        _context.Rooms.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}