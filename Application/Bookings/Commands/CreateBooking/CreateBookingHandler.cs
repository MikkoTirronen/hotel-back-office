using Domain.Aggregates.Booking;
using Domain.Abstractions.Repositories;
using Application.DTOs;
using Domain.Aggregates.Customer;
namespace Application.Bookings.Commands.CreateBooking;

public class CreateBookingHandler
{
    private readonly IBookingRepository _bookingRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly IRoomRepository _roomRepo;

    public CreateBookingHandler(
        IBookingRepository bookingRepo,
        ICustomerRepository customerRepo,
        IRoomRepository roomRepo
        )
    {
        _bookingRepo = bookingRepo;
        _customerRepo = customerRepo;
        _roomRepo = roomRepo;
    }

    public async Task<BookingDto> Handle(CreateBookingCommand command)
    {
        var dto = command.Dto;

        var customer = await _customerRepo.GetByEmailAsync(dto.Email) ??
                       new Customer(dto.Name, dto.Email);

        if (!string.IsNullOrEmpty(dto.Phone))
            customer.UpdateContactInfo(null, null, dto.Phone);

        if (customer.CustomerId == 0)
            await _customerRepo.CreateAsync(customer);


        var room = await _roomRepo.GetByIdAsync(dto.RoomId)
                   ?? throw new ApplicationException("Invalid room ID");


        var overlapping = await _bookingRepo.GetBookingsInDateRangeAsync(dto.StartDate, dto.EndDate);

        if (overlapping.Any(b => b.RoomId == dto.RoomId))
            throw new ApplicationException("Room not available");


        var booking = new Booking(dto.RoomId, customer.CustomerId, dto.StartDate, dto.EndDate, dto.NumPersons, dto.ExtraBedsCount ?? 0);

        booking.CalculateTotal(room.PricePerNight);

        booking.GenerateInvoice(DateTime.UtcNow);

        await _bookingRepo.CreateAsync(booking);


        return new BookingDto(booking);
    }
}
