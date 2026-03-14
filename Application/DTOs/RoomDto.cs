using System.Runtime.CompilerServices;
using Domain.Aggregates.RoomAggregate;
namespace Application.DTOs;

public class RoomDto
{
    public RoomDto(Room room)
    {
        RoomId = room.RoomId;
        RoomNumber = room.RoomNumber;
        PricePerNight = room.PricePerNight;
        BaseCapacity = room.BaseCapacity;
        MaxExtraBeds = room.MaxExtraBeds;
        Active = room.Active;
        amenities = room.Amenities;
    }

    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = null!;
    public decimal PricePerNight { get; set; }
    public int BaseCapacity { get; set; }
    public int MaxExtraBeds { get; set; }
    IEnumerable<string>? amenities = null;
    public bool Active { get; set; }
}