using Domain.Aggregates.BookingAggregate;
using Domain.Aggregates.CustomerAggregate;
using Domain.Aggregates.RoomAggregate;
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
        modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.RoomId).IsRequired();
                entity.Property(b => b.CustomerId).IsRequired();
                entity.Property(b => b.StartDate).IsRequired();
                entity.Property(b => b.EndDate).IsRequired();
                entity.Property(b => b.NumPersons).IsRequired();
                entity.Property(b => b.ExtraBedsCount).IsRequired();
                entity.Property(b => b.TotalPrice).IsRequired();
                entity.Property(b => b.Status)
                    .HasConversion<string>()
                    .IsRequired();

                // Map backing field for Invoice if you want EF to store it
                entity.OwnsOne(typeof(Invoice), "_invoice", inv =>
                {
                    inv.Property("Amount").HasColumnName("InvoiceAmount");
                    inv.Property("IssueDate").HasColumnName("InvoiceDate");
                });

                entity.HasOne<Customer>() // Booking -> Customer
                    .WithMany(c => c.Bookings)
                    .HasForeignKey(b => b.CustomerId);
            });

        // Customer Aggregate
        modelBuilder.Entity<Customer>(c =>
        {
            c.HasKey(x => x.CustomerId);
            c.Property(x => x.Name).IsRequired().HasMaxLength(100);
            c.Property(x => x.Email).IsRequired().HasMaxLength(100);
            c.Property(x => x.Phone).HasMaxLength(20);
        });

        // Room Aggregate
        modelBuilder.Entity<Room>(r =>
        {
            r.HasKey(x => x.RoomId);
            r.Property(x => x.PricePerNight).IsRequired();
            r.Property(x => x.BaseCapacity).IsRequired();
            r.Property(x => x.MaxExtraBeds).IsRequired();
            r.Property(x => x.Active).IsRequired();
        });
    }
}