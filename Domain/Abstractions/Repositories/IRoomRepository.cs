using Domain.Aggregates.RoomAggregate;
namespace Domain.Abstractions.Repositories;

public interface IRoomRepository : IRepositoryBase<Room, int>
{
    Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default);
}