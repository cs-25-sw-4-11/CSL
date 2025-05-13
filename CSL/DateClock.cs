namespace CSL;

public readonly struct DateClock(Date? date, Clock? clock)
{
    public Date? Date { get; } = date;
    public Clock? Clock { get; } = clock;
}
