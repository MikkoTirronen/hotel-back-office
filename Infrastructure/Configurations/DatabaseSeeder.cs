using Domain.Aggregates.BookingAggregate;
using Domain.Aggregates.CustomerAggregate;
using Domain.Aggregates.RoomAggregate;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public static class DbSeeder
{
    public static async Task SeedAsync(HotelDbContext context)
    {
        if (await context.Customers.AnyAsync()) return; // Already seeded

        // --- Customers ---
        var customers = new List<Customer>
        {
            new Customer("Alice Johnson", "alice@example.com") { },
            new Customer("Bob Smith", "bob@example.com") { },
            new Customer("Charlie Lee", "charlie@example.com") { }
        };

        await context.Customers.AddRangeAsync(customers);

        // --- Rooms ---
        var rooms = new List<Room>
        {
            new Room(0, "101", RoomType.Single, 1, 0, 50m),
            new Room(0, "102", RoomType.Double, 2, 1, 80m),
            new Room(0, "103", RoomType.Double, 2, 1, 80m)
};

        await context.Rooms.AddRangeAsync(rooms);

        // --- Bookings ---
        var booking1 = new Booking(
            roomId: rooms[0].RoomId,
            customerId: customers[0].CustomerId,
            startDate: DateTime.UtcNow.Date,
            endDate: DateTime.UtcNow.Date.AddDays(2),
            numPersons: 1
        );
        var booking2 = new Booking(
            roomId: rooms[1].RoomId,
            customerId: customers[1].CustomerId,
            startDate: DateTime.UtcNow.Date.AddDays(1),
            endDate: DateTime.UtcNow.Date.AddDays(3),
            numPersons: 2,
            extraBeds: 1
        );

        await context.Bookings.AddRangeAsync(booking1, booking2);

        await context.SaveChangesAsync();
    }
}