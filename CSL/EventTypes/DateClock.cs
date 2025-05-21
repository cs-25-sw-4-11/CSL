namespace CSL.EventTypes;

public readonly struct DateClock(Date date, Clock clock)
{
    public Date Date { get; } = date;
    public Clock Clock { get; } = clock;

    public static DateClock operator +(DateClock left, Duration right)
    {
        Duration duration = left.Date.GetDateAsDuration() +
                            left.Clock.GetClockAsDuration() + right;

        return duration.GetDurationAsDateClock();
    }

    public static DateClock operator -(DateClock left, Duration right)
    {
        Duration duration = left.Date.GetDateAsDuration() +
                            left.Clock.GetClockAsDuration() - right;
        return duration.GetDurationAsDateClock();
    }

    public static bool operator >=(DateClock left, Duration right)
    {
        DateClock rightDate = right.GetDurationAsDateClock();
        if (left.Date == rightDate.Date) return left.Clock >= rightDate.Clock;
        return left.Date >= rightDate.Date;
    }

    public static bool operator <=(DateClock left, Duration right)
    {
        DateClock rightDate = right.GetDurationAsDateClock();
        if (left.Date == rightDate.Date) return left.Clock <= rightDate.Clock;
        return left.Date <= rightDate.Date;
    }
}