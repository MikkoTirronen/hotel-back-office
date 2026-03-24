using Domain.Abstractions.Repositories;
using Domain.Aggregates.Booking;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly HotelDbContext _context;

    public BookingRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Booking?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BookingId == id, ct);
    }

    public async Task<Booking?> GetBookingDetailsAsync(int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.Invoice)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public async Task<IReadOnlyList<Booking>> GetBookingsInDateRangeAsync(DateTime start, DateTime end, CancellationToken ct = default)
    {
        return await _context.Bookings
            .Where(b => b.StartDate < end && b.EndDate > start)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Booking>> SearchAsync(
        string? customer,
        string? room,
        int? bookingId,
        DateTime? startDate,
        DateTime? endDate,
        int? guests,
        CancellationToken ct = default)
    {
        var query = _context.Bookings.AsQueryable();

        if (!string.IsNullOrEmpty(customer))
        {
            query = query.Where(b => b.CustomerId.ToString().Contains(customer)); // Adjust if you have Customer.Name navigation
        }

        if (!string.IsNullOrEmpty(room))
        {
            query = query.Where(b => b.RoomId.ToString().Contains(room)); // Adjust if you have Room navigation
        }

        if (bookingId.HasValue)
            query = query.Where(b => b.BookingId == bookingId.Value);

        if (startDate.HasValue)
            query = query.Where(b => b.StartDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(b => b.EndDate <= endDate.Value);

        if (guests.HasValue)
            query = query.Where(b => b.NumPersons == guests.Value);

        return await query.AsNoTracking().ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Booking>> GetBookingsWithUnpaidInvoicesOlderThanAsync(DateTime threshold)
    {
        return await _context.Bookings
            .Where(b => b.Invoice != null && !b.Invoice.Status.Equals("Paid") && b.Invoice.IssueDate < threshold)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task CreateAsync(Booking entity, CancellationToken ct = default)
    {
        await _context.Bookings.AddAsync(entity, ct);
    }

    public Task UpdateAsync(Booking entity, CancellationToken ct = default)
    {
        _context.Bookings.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Booking entity, CancellationToken ct = default)
    {
        _context.Bookings.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}