namespace Domain.Abstractions.Repositories;

using Domain.Aggregates.Booking;

public interface IPaymentRepository : IRepositoryBase<PaymentRecord, int>
{
    Task<IReadOnlyList<PaymentRecord>> GetAllOrderedAsync(
        CancellationToken ct = default);
}