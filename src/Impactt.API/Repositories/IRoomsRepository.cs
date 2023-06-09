using Impactt.API.Entities;

namespace Impactt.API.Repositories
{
    public interface IRoomsRepository
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<Room> GetRoomAsync(long id);
    }
}