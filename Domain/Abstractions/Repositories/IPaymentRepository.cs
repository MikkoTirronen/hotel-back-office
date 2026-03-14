namespace Domain.Abstractions.Repositories;

using Domain.Aggregates.BookingAggregate;

public interface IPaymentRepository : IRepositoryBase<PaymentRecord, int>
{
    Task<IReadOnlyList<PaymentRecord>> GetAllOrderedAsync(
        CancellationToken ct = default);
}