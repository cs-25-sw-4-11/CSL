namespace CSL;

public readonly struct Clock(int hours, int minutes)
{
    public int Hours { get; } = hours;
    public int Minutes { get; } = minutes;

    public override string ToString() => $"{Hours}:{Minutes}";

    public Duration GetClockAsDuration() => Duration.FromHours(Hours)
                                            + Duration.FromMinutes(Minutes);

    public static Clock operator +(Clock left, Duration right)
    {
        Duration duration = left.GetClockAsDuration() +
                            right;

        return duration.GetDurationAsClock();
    }

    public static Clock operator -(Clock left, Duration right)
    {
        Duration duration = left.GetClockAsDuration() - right;
        return duration.GetDurationAsClock();
    }

    public static bool operator >=(Clock left, Duration right)
    {
        return left.GetClockAsDuration() >= right;
    }

    public static bool operator <=(Clock left, Duration right)
    {
        return left.GetClockAsDuration() <= right;
    }
    
    public static Duration TildeOp(Clock left, Clock right)
{
    Duration leftDuration = left.GetClockAsDuration();
    Duration rightDuration = right.GetClockAsDuration();

    Duration difference = rightDuration - leftDuration;

    if (difference >= Duration.Zero)
        return difference;
    else
        throw new InvalidOperationException("Range operation results in negative duration");
}
}