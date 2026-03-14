namespace Application.DTOs;

using Domain.Aggregates.CustomerAggregate;
public class CustomerDto
{
    public CustomerDto(Customer customer)
    {
        CustomerId = customer.CustomerId;
        Name = customer.Name;
        Email = customer.Email;
        Phone = customer.Phone;
        //Address = customer.Address;
    }
    public int CustomerId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    //public string? Address { get; set; }
}