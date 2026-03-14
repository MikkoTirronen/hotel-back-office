using Application.DTOs;
using Domain.Abstractions.Repositories;
namespace Application.Bookings.Queries.GetBookingById;

public class GetBookingByIdHandler : IRequestHandler<GetBookingByIdQuery, BookingDto?>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public GetBookingByIdHandler(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        ICustomerRepository customerRepository,
        IInvoiceRepository invoiceRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _customerRepository = customerRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<BookingDto?> Handle(GetBookingByIdQuery query, CancellationToken ct)
    {
        var booking = await _bookingRepository.GetBookingDetailsAsync(query.BookingId);
        if (booking == null) return null;

        var room = await _roomRepository.GetByIdAsync(booking.RoomId);
        var customer = await _customerRepository.GetByIdAsync(booking.CustomerId);
        var invoice = await _invoiceRepository.GetByBookingIdAsync(booking.BookingId);

        return new BookingDto(booking)
        {
            Room = room != null ? new RoomDto(room) : null,
            Customer = customer != null ? new CustomerDto(customer) : null,
            Invoice = invoice != null ? new InvoiceDto(invoice) : null
        };
    }
}