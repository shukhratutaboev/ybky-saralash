using Impactt.API.Entities;
using Impactt.API.Models;

namespace Impactt.API.Mappers;

public static class EntityToModelMapper
{
    public static RoomModel ToModel(this Room room)
    {
        return new RoomModel
        {
            Id = room.Id,
            Name = room.Name,
            Type = room.Type,
            Capacity = room.Capacity
        };
    }

    public static BookedTimeModel ToModel(this BookedTime bookedTime)
    {
        return new BookedTimeModel
        {
            Resident = new ResidentModel()
            {
                Name = bookedTime.Resident
            },
            Start = bookedTime.StartTime,
            End = bookedTime.EndTime
        };
    }
}