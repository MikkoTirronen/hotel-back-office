using Domain.Abstractions.Repositories;
using Domain.Aggregates.Customer;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly HotelDbContext _context;

    public CustomerRepository(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Customers
            .Include(c => c.Bookings) // Optional: include bookings if needed
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == id, ct);
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == email, ct);
    }

    public async Task<IReadOnlyList<Customer>> SearchCustomersAsync(string search, CancellationToken ct = default)
    {
        var query = _context.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search) || c.Email.Contains(search) ||
                                     (c.Phone != null && c.Phone.Contains(search)));
        }

        return await query.AsNoTracking().ToListAsync(ct);
    }

    public async Task CreateAsync(Customer entity, CancellationToken ct = default)
    {
        await _context.Customers.AddAsync(entity, ct);
    }

    public Task UpdateAsync(Customer entity, CancellationToken ct = default)
    {
        _context.Customers.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Customer entity, CancellationToken ct = default)
    {
        _context.Customers.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}