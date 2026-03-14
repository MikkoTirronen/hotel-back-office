namespace Domain.Abstractions.Repositories;

using Domain.Aggregates.CustomerAggregate;
public interface ICustomerRepository : IRepositoryBase<Customer, int>
{
    Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<IReadOnlyList<Customer>> SearchCustomersAsync(string search, CancellationToken ct = default);
}