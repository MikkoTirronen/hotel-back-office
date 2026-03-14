public class GetBookingByIdQuery : IRequest<BookingDto>
{
    public int BookingId { get; }

    public GetBookingByIdQuery(int bookingId)
    {
        BookingId = bookingId;
    }
}