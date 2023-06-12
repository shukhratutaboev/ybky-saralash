using Impactt.API.Models;

namespace Impactt.API.Services;

public interface IBookingService
{
    Task<Pagination<RoomModel>> GetRoomsAsync(GetRoomsQueryModel query);
    Task<RoomModel> GetRoomAsync(long id);
    Task<IEnumerable<AvailableTimeModel>> GetRoomAvailableTimes(long id, DateOnly date);
}