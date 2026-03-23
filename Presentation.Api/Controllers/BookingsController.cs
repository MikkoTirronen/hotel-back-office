using Application.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly BookingService _bookingService;
    private readonly RoomService _roomService;

    public BookingsController(
        BookingService bookingService,
        RoomService roomService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
    }

    // --- Create a new booking ---
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingDto dto)
    {
        var bookingId = await _bookingService.CreateBookingAsync(dto);
        return Ok(new { BookingId = bookingId });
    }

    // --- Get booking by ID ---
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null) return NotFound();

        return Ok(booking);
    }

    // --- Cancel a booking ---
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        await _bookingService.CancelBookingAsync(id);
        return NoContent();
    }

    // --- Update a booking ---
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDto dto)
    {
        await _bookingService.UpdateBookingAsync(
            id,
            dto.StartDate,
            dto.EndDate,
            dto.NumPersons ?? 1,
            dto.ExtraBedsCount?? 0
        );

        return NoContent();
    }

    // --- Get available rooms for a date range ---
    [HttpGet("available-rooms")]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var rooms = await _roomService.GetAvailableRoomsAsync(startDate, endDate);
        return Ok(rooms);
    }
}