namespace Domain.Aggregates.CustomerAggregate;

using Domain.Aggregates.BookingAggregate;
public class Customer
{
    public int CustomerId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string? Phone { get; private set; }

    private readonly List<Booking> _bookings = new();
    public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

    public Customer(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public void UpdateContactInfo(string? name, string? email, string? phone)
    {
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (!string.IsNullOrEmpty(email)) Email = email;
        Phone = phone;
    }

    internal void AddBooking(Booking booking) => _bookings.Add(booking);
}