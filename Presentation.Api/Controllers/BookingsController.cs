using Application.Services;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IRoomService _roomService;

    public BookingsController(
        IBookingService bookingService,
        IRoomService roomService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingDto dto, CancellationToken ct)
    {
        var bookingId = await _bookingService.CreateBookingAsync(dto, ct);
        return Ok(new { BookingId = bookingId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(int id, CancellationToken ct)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id, ct);
        if (booking == null) return NotFound();

        return Ok(booking);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id, CancellationToken ct)
    {
        await _bookingService.CancelBookingAsync(id, ct);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDto dto, CancellationToken ct)
    {
        await _bookingService.UpdateBookingAsync(
            id,
            dto.StartDate,
            dto.EndDate,
            dto.NumPersons ?? 1,
            dto.ExtraBedsCount ?? 0,
            ct
        );

        return NoContent();
    }

    [HttpGet("available-rooms")]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken ct)
    {
        var rooms = await _roomService.GetAvailableRoomsAsync(startDate, endDate, ct);
        return Ok(rooms);
    }
}