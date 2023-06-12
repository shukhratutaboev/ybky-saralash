using Impactt.API.Entities;
using Impactt.API.Models;

namespace Impactt.API.Helpers;

public static class TimePeriodHelper
{
    public static IEnumerable<AvailableTimeModel> GetAvailableTimes(
        this IEnumerable<BookedTime> bookedTimes,
        DateOnly date)
    {
        var availableTimes = new List<AvailableTimeModel>();

        var left = date.ToDateTime(TimeOnly.MinValue);
        var right = left.AddDays(1);

        var lastEndTime = left;

        foreach (var bookedTime in bookedTimes)
        {
            if (bookedTime.StartTime > lastEndTime)
            {
                availableTimes.Add(new AvailableTimeModel
                {
                    Start = lastEndTime,
                    End = bookedTime.StartTime
                });
            }

            lastEndTime = bookedTime.EndTime;
        }

        if (lastEndTime < right)
        {
            availableTimes.Add(new AvailableTimeModel
            {
                Start = lastEndTime,
                End = right
            });
        }

        return availableTimes;
    }
}