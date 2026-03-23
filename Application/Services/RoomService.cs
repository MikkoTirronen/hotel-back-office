using Domain.Abstractions.Repositories;
using Domain.Aggregates.RoomAggregate;
using Domain.Enums;

namespace Application.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepo;

    public RoomService(IRoomRepository roomRepo)
    {
        _roomRepo = roomRepo;
    }

    // --- Create a new room ---
    public async Task<int> CreateRoomAsync(
        string roomNumber,
        RoomType type,
        int baseCapacity,
        int maxExtraBeds,
        decimal pricePerNight)
    {
        var room = new Room(
            roomId: 0,          // EF Core will generate PK
            roomNumber: roomNumber,
            type: type,
            baseCapacity: baseCapacity,
            maxExtraBeds: maxExtraBeds,
            pricePerNight: pricePerNight
        );

        await _roomRepo.CreateAsync(room);
        await _roomRepo.SaveAsync();

        return room.RoomId;
    }

    // --- Update room details ---
    public async Task UpdateRoomAsync(
        int roomId,
        string? roomNumber = null,
        RoomType? type = null,
        int? baseCapacity = null,
        int? maxExtraBeds = null,
        decimal? pricePerNight = null,
        bool? active = null)
    {
        var room = await _roomRepo.GetByIdAsync(roomId)
                   ?? throw new Exception("Room not found");

        if (!string.IsNullOrEmpty(roomNumber))
            room.SetRoomNumber(roomNumber);

        if (type.HasValue)
            room.SetRoomType(type.Value);

        if (baseCapacity.HasValue || maxExtraBeds.HasValue)
            room.UpdateCapacity(baseCapacity ?? room.BaseCapacity, maxExtraBeds ?? room.MaxExtraBeds);

        if (pricePerNight.HasValue)
            room.SetPrice(pricePerNight.Value);

        if (active.HasValue)
            room.SetActive(active.Value);
            
        await _roomRepo.UpdateAsync(room);
        await _roomRepo.SaveAsync();
    }

    // --- Get all rooms ---
    public async Task<IReadOnlyList<Room>> GetAllRoomsAsync()
    {
        return await _roomRepo.GetAllAsync();
    }

    // --- Get available rooms for a date range ---
    public async Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate)
    {
        return await _roomRepo.GetAvailableRoomsAsync(startDate, endDate);
    }

    // --- Add an amenity ---
    public async Task AddAmenityAsync(int roomId, string amenity)
    {
        var room = await _roomRepo.GetByIdAsync(roomId)
                   ?? throw new Exception("Room not found");

        room.AddAmenity(amenity);

        await _roomRepo.UpdateAsync(room);
        await _roomRepo.SaveAsync();
    }

    // --- Remove an amenity ---
    public async Task RemoveAmenityAsync(int roomId, string amenity)
    {
        var room = await _roomRepo.GetByIdAsync(roomId)
                   ?? throw new Exception("Room not found");

        room.RemoveAmenity(amenity);

        await _roomRepo.UpdateAsync(room);
        await _roomRepo.SaveAsync();
    }
}