using Impactt.API.Data;
using Impactt.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Impactt.API.Repositories;

public class RoomsRepository : IRoomsRepository
{
    private readonly ILogger<RoomsRepository> _logger;
    private readonly ImpacttDB _context;

    public RoomsRepository(
        ILogger<RoomsRepository> logger,
        ImpacttDB context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Room> GetRoomAsync(long id)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Room>> GetRoomsAsync()
    {
        return await _context.Rooms
            .ToListAsync();
    }
}