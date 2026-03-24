using Domain.Aggregates.Booking;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Room;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class HotelDbContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;

    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelDbContext).Assembly);
    }
}