using Application.DTOs;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.Bookings.Queries.GetBookingById;

public class GetBookingByIdHandler
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ICustomerRepository _customerRepository;

    public GetBookingByIdHandler(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        ICustomerRepository customerRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _customerRepository = customerRepository;
    }

    public async Task<BookingDto?> HandleAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetBookingDetailsAsync(bookingId);
        if (booking == null) return null;

        var room = await _roomRepository.GetByIdAsync(booking.RoomId);
        var customer = await _customerRepository.GetByIdAsync(booking.CustomerId);

        var invoice = booking.Invoice;

        return new BookingDto(booking)
        {
            Room = room != null ? new RoomDto(room) : null,
            Customer = customer != null ? new CustomerDto(customer) : null,
            Invoice = invoice != null ? new InvoiceDto(invoice) : null
        };
    }
}