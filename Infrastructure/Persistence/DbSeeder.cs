using Domain.Aggregates.Booking;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Room;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DbSeeder
{
    public static void SeedDatabase(HotelDbContext context)
    {
        // Ensure database exists
        context.Database.EnsureCreated();

        // -----------------------
        // Seed Rooms
        // -----------------------
        if (!context.Set<Room>().Any())
        {
            var rooms = new List<Room>
            {
                new Room( "101", RoomType.Standard, 2, 1, 100m),
                new Room( "102", RoomType.Deluxe, 2, 2, 150m),
                new Room( "103", RoomType.Suite, 4, 2, 250m)
            };

            rooms[0].AddAmenity("TV");
            rooms[0].AddAmenity("WiFi");
            rooms[1].AddAmenity("TV");
            rooms[1].AddAmenity("WiFi");
            rooms[1].AddAmenity("Mini Bar");
            rooms[2].AddAmenity("TV");
            rooms[2].AddAmenity("WiFi");
            rooms[2].AddAmenity("Mini Bar");
            rooms[2].AddAmenity("Jacuzzi");

            context.Set<Room>().AddRange(rooms);
        }

        // -----------------------
        // Seed Customers
        // -----------------------
        if (!context.Set<Customer>().Any())
        {
            var customers = new List<Customer>
            {
                new Customer("Alice Johnson", "alice@example.com"),
                new Customer("Bob Smith", "bob@example.com"),
                new Customer("Carol Davis", "carol@example.com")
            };
            context.Set<Customer>().AddRange(customers);
        }

        context.SaveChanges();

        // -----------------------
        // Seed Bookings + Invoices + Payments
        // -----------------------
        if (!context.Set<Booking>().Any())
        {
            var room101 = context.Set<Room>().First(r => r.RoomNumber == "101");
            var room102 = context.Set<Room>().First(r => r.RoomNumber == "102");
            var alice = context.Set<Customer>().First(c => c.Name == "Alice Johnson");
            var bob = context.Set<Customer>().First(c => c.Name == "Bob Smith");

            var booking1 = new Booking(
                room101.RoomId,
                alice.CustomerId,
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(3),
                numPersons: 2,
                extraBedsCount: 0
            );
            booking1.CalculateTotal(100m);

            var booking2 = new Booking(
                room102.RoomId,
                bob.CustomerId,
                DateTime.Today.AddDays(5),
                DateTime.Today.AddDays(8),
                numPersons: 2,
                extraBedsCount: 1
            );
            booking2.CalculateTotal(100m);
            // Optional: Create invoice for booking1
            booking1.GenerateInvoice(DateTime.Today.AddDays(-1));
            booking2.GenerateInvoice(DateTime.Today);

            // Optional: Add payments
            var invoice1 = booking1.Invoice;
            invoice1!.AddPayment(50m, DateTime.Today, "Credit Card");

            var invoice2 = booking2.Invoice;
            invoice2!.AddPayment(100m, DateTime.Today, "Cash");

            context.Set<Booking>().AddRange(booking1, booking2);
        }

        context.SaveChanges();
    }
}