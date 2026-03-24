using Domain.Aggregates.Room;

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
        Amenities = room.Amenities.Select(a => a.Value).ToList();
    }

    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = null!;
    public decimal PricePerNight { get; set; }
    public int BaseCapacity { get; set; }
    public int MaxExtraBeds { get; set; }
    public bool Active { get; set; }

    public IEnumerable<string> Amenities { get; set; } = new List<string>();
}