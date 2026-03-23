using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Aggregates.RoomAggregate;

public class Room
{
    public Room(int roomId, string roomNumber, RoomType type, int baseCapacity, int maxExtraBeds, decimal pricePerNight)
    {
        RoomId = roomId;
        RoomNumber = roomNumber;
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

    public void SetRoomNumber(string roomNumber)
    {
        if (string.IsNullOrWhiteSpace(roomNumber))
            throw new DomainException("Room number cannot be empty.");
        RoomNumber = roomNumber;
    }

    public void SetRoomType(RoomType type) => Type = type;

    public void UpdateCapacity(int baseCapacity, int maxExtraBeds)
    {
        if (baseCapacity <= 0) throw new DomainException("Base capacity must be positive.");
        if (maxExtraBeds < 0) throw new DomainException("Max extra beds cannot be negative.");
        BaseCapacity = baseCapacity;
        MaxExtraBeds = maxExtraBeds;
    }

    public void SetPrice(decimal pricePerNight)
    {
        if (pricePerNight < 0) throw new DomainException("Price cannot be negative.");
        PricePerNight = pricePerNight;
    }

    public void SetActive(bool active) => Active = active;
    public void AddAmenity(string amenity)
    {
        if (!_amenities.Contains(amenity))
            _amenities.Add(amenity);
    }

    public void RemoveAmenity(string amenity) => _amenities.Remove(amenity);
}