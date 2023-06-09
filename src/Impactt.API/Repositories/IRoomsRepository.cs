using Impactt.API.Entities;

namespace Impactt.API.Repositories
{
    public interface IRoomsRepository
    {
        Task<IQueryable<Room>> GetRoomsQueryableAsync();
        Task<Room> GetRoomAsync(long id);
    }
}