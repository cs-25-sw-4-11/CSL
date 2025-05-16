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
}