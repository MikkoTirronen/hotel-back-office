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

    // Create a new customer
    public async Task<int> CreateCustomerAsync(string name, string email, string? phone = null)
    {
        var existing = await _customerRepo.GetByEmailAsync(email);
        if (existing != null) throw new Exception("Email already in use");

        var customer = new Customer(name, email);
        if (!string.IsNullOrEmpty(phone))
            customer.UpdateContactInfo(null, null, phone);

        await _customerRepo.CreateAsync(customer);
        await _customerRepo.SaveAsync();

        return customer.CustomerId;
    }

    // Update contact info
    public async Task UpdateCustomerAsync(int customerId, string? name, string? email, string? phone)
    {
        var customer = await _customerRepo.GetByIdAsync(customerId)
                       ?? throw new Exception("Customer not found");

        if (!string.IsNullOrEmpty(email))
        {
            var exists = await _customerRepo.GetByEmailAsync(email);
            if (exists != null && exists.CustomerId != customerId)
                throw new Exception("Email already in use");
        }

        customer.UpdateContactInfo(name, email, phone);

        await _customerRepo.UpdateAsync(customer);
        await _customerRepo.SaveAsync();
    }

    // Search customers
    public async Task<IReadOnlyList<Customer>> SearchCustomersAsync(string search)
    {
        return await _customerRepo.SearchCustomersAsync(search);
    }

    // Get all customers
    public async Task<IReadOnlyList<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepo.GetAllAsync();
    }
}