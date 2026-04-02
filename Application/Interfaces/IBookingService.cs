using Application.Common;
using Application.DTOs;
using Domain.Aggregates.Booking;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<Result<int>> CreateBookingAsync(BookingDto dto, CancellationToken ct);
        Task<Booking?> GetBookingByIdAsync(int bookingId, CancellationToken ct);
        Task CancelBookingAsync(int bookingId, CancellationToken ct);
        Task UpdateBookingAsync(int bookingId, DateTime newStart, DateTime newEnd, int guests, int extraBeds = 0, CancellationToken ct = default);
    }
}