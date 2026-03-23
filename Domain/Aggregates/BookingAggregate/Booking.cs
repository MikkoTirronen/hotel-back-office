using Domain.Enums;
using Domain.Exceptions;
namespace Domain.Aggregates.BookingAggregate;

public class Booking
{
    public int BookingId { get; private set; }
    public int RoomId { get; private set; }
    public int CustomerId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int NumPersons { get; private set; }
    public int ExtraBedsCount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; } = BookingStatus.Pending;
    private Invoice? _invoice;
    public Invoice? Invoice => _invoice;
    public Booking(int roomId, int customerId, DateTime startDate, DateTime endDate, int numPersons, int extraBeds = 0)
    {
        if (endDate <= startDate) throw new DomainException("End date must be after start date.");
        if (numPersons <= 0) throw new DomainException("Number of persons must be at least 1.");

        RoomId = roomId;
        CustomerId = customerId;
        StartDate = startDate;
        EndDate = endDate;
        NumPersons = numPersons;
        ExtraBedsCount = extraBeds;
        Status = BookingStatus.Pending;
        _invoice = new Invoice(TotalPrice, DateTime.UtcNow);
    }
    public void UpdateDates(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new DomainException("End date must be after start date.");
        StartDate = start;
        EndDate = end;
    }

    public void UpdateGuests(int guests, int extraBeds = 0)
    {
        if (guests <= 0)
            throw new DomainException("Number of persons must be at least 1.");

        if (extraBeds < 0)
            throw new DomainException("Extra beds cannot be negative.");

        NumPersons = guests;
        ExtraBedsCount = extraBeds;
    }

    public void CalculateTotal(decimal pricePerNight)
    {
        int nights = (EndDate - StartDate).Days;
        if (nights <= 0) nights = 1;
        TotalPrice = pricePerNight * nights;
    }
    public void ChangeRoom(int roomId)
    {
        if (Status == BookingStatus.Confirmed)
            throw new DomainException("Cannot change room after confirmation.");

        RoomId = roomId;
    }
    public void Cancel()
    {
        if (Status == BookingStatus.Canceled)
            throw new DomainException("Booking is already canceled.");

        Status = BookingStatus.Canceled;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new DomainException("Only pending bookings can be confirmed.");

        Status = BookingStatus.Confirmed;
    }

public void GenerateInvoice(DateTime issueDate)
{
    if (_invoice != null)
        throw new DomainException("Invoice already exists");

    _invoice = new Invoice(TotalPrice, issueDate);
}
}