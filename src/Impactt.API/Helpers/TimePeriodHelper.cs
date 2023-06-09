using Impactt.API.Entities;

namespace Impactt.API.Helpers;

public static class TimePeriodHelper
{
    public static bool IsTimePeriodAvailable(
        DateTime start, DateTime end,
        IEnumerable<BookedTime> bookedTimes)
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