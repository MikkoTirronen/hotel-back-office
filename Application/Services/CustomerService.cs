using Application.Interfaces;
using Domain.Abstractions.Repositories;
using Domain.Aggregates.Customer;

namespace Application.Services;

public class CustomerService: ICustomerService
{
    private readonly ICustomerRepository _customerRepo;

    public CustomerService(ICustomerRepository customerRepo)
    {
        _customerRepo = customerRepo;
    }

    public async Task<int> CreateCustomerAsync(string name, string email, string? phone = null, CancellationToken ct = default)
    {
        var existing = await _customerRepo.GetByEmailAsync(email, ct);
        if (existing != null) throw new Exception("Email already in use");

        var customer = new Customer(name, email);
        if (!string.IsNullOrEmpty(phone))
            customer.UpdateContactInfo(null, null, phone);

        await _customerRepo.CreateAsync(customer, ct);
        await _customerRepo.SaveAsync(ct);

        return customer.CustomerId;
    }

    public async Task UpdateCustomerAsync(int customerId, string? name, string? email, string? phone, CancellationToken ct = default)
    {
        var customer = await _customerRepo.GetByIdAsync(customerId,ct)
                       ?? throw new Exception("Customer not found");

        if (!string.IsNullOrEmpty(email))
        {
            var exists = await _customerRepo.GetByEmailAsync(email, ct);
            if (exists != null && exists.CustomerId != customerId)
                throw new Exception("Email already in use");
        }

        customer.UpdateContactInfo(name, email, phone);

        await _customerRepo.UpdateAsync(customer,ct);
        await _customerRepo.SaveAsync(ct);
    }

    public async Task<IReadOnlyList<Customer>> SearchCustomersAsync(string search, CancellationToken ct = default)
    {
        return await _customerRepo.SearchCustomersAsync(search, ct);
    }

    public async Task<IReadOnlyList<Customer>> GetAllCustomersAsync(CancellationToken ct = default)
    {
        return await _customerRepo.GetAllAsync(ct);
    }
}