using Domain.Enums;

namespace Domain.Aggregates.RoomAggregate;

public class Room
{
    public Room(int roomId, RoomType type, int baseCapacity, int maxExtraBeds, decimal pricePerNight)
    {
        RoomId = roomId;
        Type = type;
        BaseCapacity = baseCapacity;
        MaxExtraBeds = maxExtraBeds;
        PricePerNight = pricePerNight;
    }
    public int RoomId { get; private set; }
    public string RoomNumber { get; private set; } = null!;
    public RoomType Type { get; private set; }
    public int BaseCapacity { get; private set; }
    public int MaxExtraBeds { get; private set; }
    public decimal PricePerNight { get; private set; }
    public bool Active { get; private set; } = true;

    private readonly List<string> _amenities = new();
    public IReadOnlyCollection<string> Amenities => _amenities.AsReadOnly();


    public void AddAmenity(string amenity)
    {
        if (!_amenities.Contains(amenity))
            _amenities.Add(amenity);
    }

    public void RemoveAmenity(string amenity) => _amenities.Remove(amenity);
}