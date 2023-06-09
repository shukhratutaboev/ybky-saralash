using Impactt.API.Entities;
using Impactt.API.Models;

namespace Impactt.API.Mappers
{
    public static class ModelToEntityMapper
    {
        public static BookedTime ToEntity(this BookedTimeModel bookedTime)
        {
            return new BookedTime
            {
                Resident = bookedTime.Resident.Name,
                StartTime = bookedTime.Start,
                EndTime = bookedTime.End
            };
        }
    }
}