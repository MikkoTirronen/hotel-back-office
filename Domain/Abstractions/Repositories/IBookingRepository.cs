namespace Domain.Abstractions.Repositories;

using Domain.Aggregates.Booking;
public interface IBookingRepository : IRepositoryBase<Booking, int>
{
    Task<Booking?> GetBookingDetailsAsync(int bookingId);

    Task<IReadOnlyList<Booking>> GetBookingsInDateRangeAsync(
    DateTime start,
    DateTime end,
    CancellationToken ct = default);

    Task<IReadOnlyList<Booking>> SearchAsync(
        string? customer,
        string? room,
        int? bookingId,
        DateTime? startDate,
        DateTime? endDate,
        int? guests,
        CancellationToken ct = default);

    Task<IReadOnlyList<Booking>> GetBookingsWithUnpaidInvoicesOlderThanAsync(DateTime threshold);
}