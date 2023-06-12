using Impactt.API.Data;
using Impactt.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Impactt.API.Repositories;

public class BookedTimesRepository : IBookedTimesRepository
{
    private readonly ILogger<BookedTimesRepository> _logger;
    private readonly ImpacttDB _context;

    public BookedTimesRepository(
        ILogger<BookedTimesRepository> logger,
        ImpacttDB context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<BookedTime> AddBookedTimeAsync(BookedTime bookedTime)
    {
        var entry = await _context.BookedTimes.AddAsync(bookedTime);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task<IEnumerable<BookedTime>> GetRoomBookedTimesAsync(long roomId, DateOnly date)
    {
        var left = date.ToDateTime(TimeOnly.MinValue);
        var right = left.AddDays(1);

        return await _context.BookedTimes
            .Where(e => e.RoomId == roomId &&
                ((e.StartTime >= left && e.StartTime < right) ||
                (e.EndTime > left && e.EndTime <= right) ||
                (e.StartTime <= left && e.EndTime >= right)))
            .OrderBy(e => e.StartTime)
            .ToListAsync();
    }

    public async Task<bool> IsAvailableAsync(long roomId, DateTime startTime, DateTime endTime)
    {
        var overlapping = await _context.BookedTimes
            .Where(e => e.RoomId == roomId &&
                ((e.StartTime >= startTime && e.StartTime < endTime) ||
                    (e.EndTime > startTime && e.EndTime <= endTime) ||
                    (e.StartTime <= startTime && e.EndTime >= endTime)))
            .AnyAsync();

        return !overlapping;
    }
}