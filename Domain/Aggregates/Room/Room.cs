using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Aggregates.Room;

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
    private Room() { }
    public int RoomId { get; private set; }
    public string RoomNumber { get; private set; } = null!;
    public RoomType Type { get; private set; }
    public int BaseCapacity { get; private set; }
    public int MaxExtraBeds { get; private set; }
    public decimal PricePerNight { get; private set; }
    public bool Active { get; private set; } = true;
    private readonly List<Amenity> _amenities = new();
    public IReadOnlyCollection<Amenity> Amenities => _amenities.AsReadOnly();

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
        if (_amenities.Any(a => a.Value == amenity)) return;

        _amenities.Add(new Amenity(amenity));
    }

    public void RemoveAmenity(string amenity)
    {
        var existing = _amenities.FirstOrDefault(a => a.Value == amenity);
        if (existing != null)
            _amenities.Remove(existing);
    }
}