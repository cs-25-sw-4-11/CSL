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
        return left >= right.GetDurationAsDateClock();
    }

    public static bool operator <=(DateClock left, Duration right)
    {
        return left <= right.GetDurationAsDateClock();
    }
    public static bool operator >=(DateClock left, DateClock right)
    {
        if (left.Date == right.Date) return left.Clock >= right.Clock;
        return left.Date >= right.Date;
    }

    public static bool operator <=(DateClock left, DateClock right)
    {
        if (left.Date == right.Date) return left.Clock <= right.Clock;
        return left.Date <= right.Date;
    }
}