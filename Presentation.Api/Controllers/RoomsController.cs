using Application.DTOs;
using Application.Interfaces;
using Domain.Aggregates.Room;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms(CancellationToken ct)
    {
        var rooms = await _roomService.GetAllRoomsAsync(ct);
        var dto = rooms.Select(r => new RoomDto(r));
        return Ok(dto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomDto>> GetRoomById(int id, CancellationToken ct)
    {
        var room = await _roomService.GetRoomByIdAsync(id, ct);
        if (room == null) return NotFound();
        return Ok(new RoomDto(room));
    }

    // POST: api/rooms
    [HttpPost]
    public async Task<ActionResult<int>> CreateRoom(
        [FromBody] CreateRoomDto request,
        CancellationToken ct)
    {
        var roomId = await _roomService.CreateRoomAsync(
            request.RoomNumber,
            request.Type,
            request.BaseCapacity,
            request.MaxExtraBeds,
            request.PricePerNight,
            ct);

        return CreatedAtAction(nameof(GetRoomById), new { id = roomId }, roomId);
    }

    // PUT: api/rooms/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRoom(
        int id,
        [FromBody] UpdateRoomDto request,
        CancellationToken ct)
    {
        await _roomService.UpdateRoomAsync(
            id,
            request.RoomNumber,
            request.Type,
            request.BaseCapacity,
            request.MaxExtraBeds,
            request.PricePerNight,
            request.Active,
            ct);

        return NoContent();
    }

    // DELETE: api/rooms/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRoom(int id, CancellationToken ct)
    {
        await _roomService.DeleteRoomAsync(id, ct);
        return NoContent();
    }
    public async Task<IActionResult> SetRoomActiveStatus(int id, [FromQuery] bool isActive, CancellationToken ct)
    {
        await _roomService.SetRoomActiveStatusAsync(id, isActive, ct);
        return NoContent();
    }
    // GET: api/rooms/available
    [HttpGet("available")]
    public async Task<ActionResult<IReadOnlyList<Room>>> GetAvailableRooms(
        [FromQuery] DateTime start,
        [FromQuery] DateTime end,
        CancellationToken ct)
    {
        if (start >= end)
            return BadRequest("End date must be after start date.");

        var rooms = await _roomService.GetAvailableRoomsAsync(start, end, ct);

        return Ok(rooms);
    }

    // POST: api/rooms/{id}/amenities
    [HttpPost("{id:int}/amenities")]
    public async Task<IActionResult> AddAmenity(
        int id,
        [FromBody] string amenity,
        CancellationToken ct)
    {
        await _roomService.AddAmenityAsync(id, amenity, ct);
        return NoContent();
    }

    // DELETE: api/rooms/{id}/amenities
    [HttpDelete("{id:int}/amenities")]
    public async Task<IActionResult> RemoveAmenity(
        int id,
        [FromBody] string amenity,
        CancellationToken ct)
    {
        await _roomService.RemoveAmenityAsync(id, amenity, ct);
        return NoContent();
    }
}