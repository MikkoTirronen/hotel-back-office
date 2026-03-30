using Application.DTOs;
using Domain.Aggregates.Booking;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<int> CreateBookingAsync(BookingDto dto);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task CancelBookingAsync(int bookingId);
        Task UpdateBookingAsync(int bookingId, DateTime newStart, DateTime newEnd, int guests, int extraBeds = 0);
    }
}