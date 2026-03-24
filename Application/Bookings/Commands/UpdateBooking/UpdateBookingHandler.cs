using Domain.Aggregates.Booking;
using Domain.Exceptions;
using Domain.Enums;
using Application.DTOs;
using Domain.Abstractions.Repositories;
namespace Application.Bookings.Commands.UpdateBooking;

public class UpdateBookingHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ICustomerRepository _customerRepository;

    private readonly IInvoiceRepository _invoiceRepository;
    public UpdateBookingHandler(IBookingRepository bookingRepository, IRoomRepository roomRepository, ICustomerRepository customerRepository, IInvoiceRepository invoiceRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _customerRepository = customerRepository;
        _invoiceRepository = invoiceRepository;

    }

    public async Task<BookingDto> Handle(UpdateBookingCommand command)
    {
        var booking = await _bookingRepository.GetBookingDetailsAsync(command.BookingId)
                      ?? throw new DomainException("Booking not found");

        int roomId = command.RoomId ?? booking.RoomId;

        var room = await _roomRepository.GetByIdAsync(roomId)
                   ?? throw new DomainException("Room not found");

        if (command.StartDate.HasValue && command.EndDate.HasValue)
        {
            var overlapping = await _bookingRepository.GetBookingsInDateRangeAsync(
                command.StartDate.Value,
                command.EndDate.Value
            );

            if (overlapping.Any(b => b.RoomId == roomId && b.BookingId != booking.BookingId))
                throw new DomainException("Room not available for the selected dates");

            booking.UpdateDates(command.StartDate.Value, command.EndDate.Value);
        }

        if (command.RoomId.HasValue && command.RoomId.Value != booking.RoomId)
        {
            booking.ChangeRoom(room.RoomId);
        }

        if (command.NumPersons.HasValue || command.ExtraBedsCount.HasValue)
        {
            booking.UpdateGuests(
                command.NumPersons ?? booking.NumPersons,
                command.ExtraBedsCount ?? booking.ExtraBedsCount
            );
        }

        booking.CalculateTotal(room.PricePerNight);

        await _bookingRepository.SaveAsync();

        var customer = await _customerRepository.GetByIdAsync(booking.CustomerId);

        var invoice = await _invoiceRepository.GetByBookingIdAsync(booking.BookingId);
        if (invoice != null)
        {
            invoice.UpdateAmountDue(booking.TotalPrice); // domain rule inside Invoice
            await _invoiceRepository.SaveAsync();
        }
        return new BookingDto(booking)
        {
            Room = new RoomDto(room),
            Customer = customer != null ? new CustomerDto(customer) : null,
            Invoice = invoice != null ? new InvoiceDto(invoice) : null
        };
    }
}