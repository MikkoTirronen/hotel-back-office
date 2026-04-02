using Domain.Aggregates.Room;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IRoomService
    {
        Task<int> CreateRoomAsync(
            string roomNumber,
            RoomType type,
            int baseCapacity,
            int maxExtraBeds,
            decimal pricePerNight,
            CancellationToken ct);

        Task UpdateRoomAsync(
            int roomId,
            string? roomNumber = null,
            RoomType? type = null,
            int? baseCapacity = null,
            int? maxExtraBeds = null,
            decimal? pricePerNight = null,
            bool? active = null,
            CancellationToken ct = default);

        Task<Room?> GetRoomByIdAsync(int roomId, CancellationToken ct = default);
        Task<IReadOnlyList<Room>> GetAllRoomsAsync(CancellationToken ct = default);

        Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);

        Task AddAmenityAsync(int roomId, string amenity, CancellationToken ct = default);

        Task RemoveAmenityAsync(int roomId, string amenity, CancellationToken ct = default);
        Task DeleteRoomAsync(int roomId, CancellationToken ct = default);
        Task SetRoomActiveStatusAsync(int roomId, bool isActive, CancellationToken ct = default);
    }
}