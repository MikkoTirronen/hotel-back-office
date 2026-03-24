namespace Domain.Abstractions.Repositories;

using Domain.Aggregates.Booking;
public interface IInvoiceRepository : IRepositoryBase<Invoice, int>
{
    Task<Invoice?> GetByBookingIdAsync(int bookingId, CancellationToken ct = default);

    Task<IReadOnlyList<Invoice>> GetUnpaidOlderThanAsync(
        DateTime thresholdDate,
        CancellationToken ct = default);
}