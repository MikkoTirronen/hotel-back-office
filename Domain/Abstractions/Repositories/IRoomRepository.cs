using Domain.Aggregates.Room;
namespace Domain.Abstractions.Repositories;

public interface IRoomRepository : IRepositoryBase<Room, int>
{
    Task<IReadOnlyList<Room>> GetAvailableRoomsAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default);
}