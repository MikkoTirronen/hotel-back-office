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
    public async Task<IActionResult> CreateBooking([FromBody] BookingDto dto)
    {
        var bookingId = await _bookingService.CreateBookingAsync(dto);
        return Ok(new { BookingId = bookingId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null) return NotFound();

        return Ok(booking);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        await _bookingService.CancelBookingAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDto dto)
    {
        await _bookingService.UpdateBookingAsync(
            id,
            dto.StartDate,
            dto.EndDate,
            dto.NumPersons ?? 1,
            dto.ExtraBedsCount ?? 0
        );

        return NoContent();
    }

    [HttpGet("available-rooms")]
    public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var rooms = await _roomService.GetAvailableRoomsAsync(startDate, endDate);
        return Ok(rooms);
    }
}