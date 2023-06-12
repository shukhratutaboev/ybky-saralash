using Impactt.API.Helpers;
using Impactt.API.Mappers;
using Impactt.API.Models;
using Impactt.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Impactt.API.Services;

public class BookingService : IBookingService
{
    private readonly ILogger<BookingService> _logger;
    private readonly IRoomsRepository _roomsRepository;
    private readonly IBookedTimesRepository _bookedTimesRepository;

    public BookingService(
        ILogger<BookingService> logger,
        IRoomsRepository roomsRepository,
        IBookedTimesRepository bookedTimesRepository)
    {
        _logger = logger;
        _roomsRepository = roomsRepository;
        _bookedTimesRepository = bookedTimesRepository;
    }

    public async Task<Pagination<RoomModel>> GetRoomsAsync(GetRoomsQueryModel query)
    {
        var rooms = await _roomsRepository.GetRoomsQueryableAsync();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            rooms = rooms.Where(e => e.Name.StartsWith(query.Search));
        }

        if (!string.IsNullOrWhiteSpace(query.Type))
        {
            rooms = rooms.Where(e => e.Type == query.Type);
        }

        var count = await rooms.CountAsync();

        rooms = rooms
            .OrderBy(e => e.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize);

        return new Pagination<RoomModel>
        {
            PageNumber = query.Page,
            PageSize = query.PageSize,
            Count = count,
            Results = rooms.Select(r => r.ToModel())
        };
    }

    public async Task<RoomModel> GetRoomAsync(long id)
    {
        var room = await _roomsRepository.GetRoomAsync(id);

        if (room == null)
        {
            return null;
        }

        return room.ToModel();
    }

    public async Task<IEnumerable<AvailableTimeModel>> GetRoomAvailableTimes(long id, DateOnly date)
    {
        var room = await _roomsRepository.GetRoomAsync(id);

        if (room == null)
        {
            return null;
        }

        var bookedTimes = await _bookedTimesRepository.GetRoomBookedTimesAsync(id, date);

        var availableTimes = bookedTimes.GetAvailableTimes(date);

        if (date == DateOnly.FromDateTime(DateTime.Today))
        {
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            now = now.AddMinutes(1);
            
            availableTimes = availableTimes.Where(e => e.End > now).OrderBy(e => e.Start);

            if (availableTimes.Any() && availableTimes.First().Start < now)
            {
                availableTimes.First().Start = now;
            }
        }

        return availableTimes;
    }
}