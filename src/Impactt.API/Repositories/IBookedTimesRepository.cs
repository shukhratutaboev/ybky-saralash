using Impactt.API.Entities;

namespace Impactt.API.Repositories;
public interface IBookedTimesRepository
{
    Task<IEnumerable<BookedTime>> GetRoomBookedTimesAsync(long roomId, DateOnly date);
    Task<BookedTime> AddBookedTimeAsync(BookedTime bookedTime);
    Task<bool> IsAvailableAsync(long roomId, DateTime startTime, DateTime endTime);
}