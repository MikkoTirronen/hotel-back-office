using Domain.Aggregates.BookingAggregate;
using Domain.Aggregates.CustomerAggregate;

namespace Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(HotelDbContext context)
    {
        if (context.Customers.Any()) return; // Already seeded

        // Create a few customers
        var alice = new Customer("Alice Smith", "alice@example.com");
        var bob = new Customer("Bob Johnson", "bob@example.com");

        context.Customers.AddRange(alice, bob);

        // Create bookings
        var booking1 = new Booking(
            roomId: 101,
            customerId: alice.CustomerId,
            startDate: DateTime.Today,
            endDate: DateTime.Today.AddDays(2),
            numPersons: 2
        );
        booking1.CalculateTotal(80m); // Set TotalPrice

        var booking2 = new Booking(
            roomId: 102,
            customerId: bob.CustomerId,
            startDate: DateTime.Today.AddDays(1),
            endDate: DateTime.Today.AddDays(3),
            numPersons: 1
        );
        booking2.CalculateTotal(50m);

        alice.CreateBooking(booking1);
        bob.CreateBooking(booking2);

        context.Bookings.AddRange(booking1, booking2);

        await context.SaveChangesAsync();
    }
}