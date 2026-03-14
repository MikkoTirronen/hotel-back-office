namespace Domain.Abstractions.Repositories;
public interface IRepositoryBase<TAggregate, TId>
{
    Task<IReadOnlyList<TAggregate>> GetAllAsync(CancellationToken ct = default);
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken ct= default);
    Task CreateAsync(TAggregate entity, CancellationToken ct = default);
    Task UpdateAsync(TAggregate entity, CancellationToken ct = default);
    Task DeleteAsync(TAggregate entity, CancellationToken ct = default);

    Task SaveAsync(CancellationToken ct = default);
}