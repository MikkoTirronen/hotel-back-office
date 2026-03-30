using Domain.Aggregates.Customer;

namespace Application.Services
{
    public interface ICustomerService
    {
        Task<int> CreateCustomerAsync(string name, string email, string? phone = null);

        Task UpdateCustomerAsync(int customerId, string? name, string? email, string? phone);

        Task<IReadOnlyList<Customer>> SearchCustomersAsync(string search);

        Task<IReadOnlyList<Customer>> GetAllCustomersAsync();
    }
}