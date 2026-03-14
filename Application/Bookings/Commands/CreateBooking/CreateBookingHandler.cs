using Domain.Aggregates.BookingAggregate;
using Domain.Abstractions.Repositories;
using Application.DTOs;
using Domain.Aggregates.CustomerAggregate;
namespace Application.Bookings.Commands.CreateBooking;

public class CreateBookingHandler
{
    private readonly IBookingRepository _bookingRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly IRoomRepository _roomRepo;
    private readonly IInvoiceService _invoiceService;

    public CreateBookingHandler(
        IBookingRepository bookingRepo,
        ICustomerRepository customerRepo,
        IRoomRepository roomRepo,
        IInvoiceService invoiceService)
    {
        _bookingRepo = bookingRepo;
        _customerRepo = customerRepo;
        _roomRepo = roomRepo;
        _invoiceService = invoiceService;
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


        var booking = new Booking(dto.RoomId, customer.CustomerId, dto.StartDate, dto.EndDate, dto.NumPersons, dto.ExtraBedsCount);
        booking.CalculateTotal(room.PricePerNight);

        await _bookingRepo.CreateAsync(booking);

    
        await _invoiceService.CreateInvoiceAsync(booking.BookingId, booking.TotalPrice);

        return new BookingDto(booking);
    }
}
