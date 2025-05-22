namespace CSL;

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
        return (left.Date.GetDateAsDuration() + left.Clock.GetClockAsDuration()) >= right;
    }

    public static bool operator <=(DateClock left, Duration right)
    {
        return (left.Date.GetDateAsDuration() + left.Clock.GetClockAsDuration()) <= right;
    }


    public static Duration TildeOp(DateClock left, Date right)
    {
        Duration leftDuration = left.Date.GetDateAsDuration() + left.Clock.GetClockAsDuration();
        Duration rightDuration = right.GetDateAsDuration();

        Duration difference = rightDuration - leftDuration;

        if (difference >= Duration.Zero)
            return difference;
        else
            throw new InvalidOperationException("Range operation results in negative duration");
    }

    public static Duration TildeOp(Date left, DateClock right)
    {
        Duration leftDuration = left.GetDateAsDuration();
        Duration rightDuration = right.Date.GetDateAsDuration() + right.Clock.GetClockAsDuration();

        Duration difference = rightDuration - leftDuration;

        if (difference >= Duration.Zero)
            return difference;
        else
            throw new InvalidOperationException("Range operation results in negative duration");
    }

    public static Duration TildeOp(DateClock left, DateClock right)
    {
     Duration leftDuration = left.Date.GetDateAsDuration() + left.Clock.GetClockAsDuration();
     Duration rightDuration = right.Date.GetDateAsDuration() + right.Clock.GetClockAsDuration();

     Duration difference = rightDuration - leftDuration;

    if (difference >= Duration.Zero)
        return difference;
     else
        throw new InvalidOperationException("Range operation results in negative duration");
}
}