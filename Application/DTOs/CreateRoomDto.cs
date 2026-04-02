using Domain.Enums;

namespace Application.DTOs;

public class CreateRoomDto
{
    public string RoomNumber { get; set; } = null!;
    public decimal PricePerNight { get; set; }
    public int BaseCapacity { get; set; }
    public int MaxExtraBeds { get; set; }
    public string? Amenities { get; set; }
    public RoomType Type { get; set; }
}