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
            if (bookedTime.StartTime < lastEndTime)
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

    public static bool IsTimePeriodAvailable(
        this IEnumerable<BookedTime> bookedTimes,
        DateTime start, DateTime end)
    {
        foreach (var bookedTime in bookedTimes)
        {
            if (!AreTimePeriodsNonOverlapping(start, end, bookedTime.StartTime, bookedTime.EndTime))
            {
                return false;
            }
        }

        return true;
    }

    private static bool AreTimePeriodsNonOverlapping(
        DateTime start1, DateTime end1,
        DateTime start2, DateTime end2)
    {
        if (start1 >= start2 && start1 < end2)
        {
            return false;
        }

        if (end1 > start2 && end1 <= end2)
        {
            return false;
        }

        return true;
    }
}