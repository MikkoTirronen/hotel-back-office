using Domain.Aggregates.Customer;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        Task<int> CreateCustomerAsync(string name, string email, string? phone = null, CancellationToken ct = default);

        Task UpdateCustomerAsync(int customerId, string? name, string? email, string? phone, CancellationToken ct = default);

        Task<IReadOnlyList<Customer>> SearchCustomersAsync(string search, CancellationToken ct = default);

        Task<IReadOnlyList<Customer>> GetAllCustomersAsync(CancellationToken ct = default);
    }
}