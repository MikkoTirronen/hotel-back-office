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
            decimal pricePerNight);

        Task UpdateRoomAsync(
            int roomId,
            string? roomNumber = null,
            RoomType? type = null,
            int? baseCapacity = null,
            int? maxExtraBeds = null,
            decimal? pricePerNight = null,
            bool? active = null);

        Task<IReadOnlyList<Room>> GetAllRoomsAsync();

        Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(DateTime startDate, DateTime endDate);

        Task AddAmenityAsync(int roomId, string amenity);

        Task RemoveAmenityAsync(int roomId, string amenity);
    }
}