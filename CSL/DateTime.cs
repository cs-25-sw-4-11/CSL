namespace CSL;

public readonly struct DateTime(Date? date, Clock? clock)
{
    public Date? Date { get; } = date;
    public Clock? Clock { get; } = clock;
}
