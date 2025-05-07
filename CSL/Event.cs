namespace CSL;

public readonly struct Event(Subject? subject = null, Date? date = null, Clock? clock = null, Duration? duration = null, Description? description = null)
{
    public Subject? Subject { get; } = subject;
    public Date? Date { get; } = date;
    public Clock? Clock { get; } = clock;
    public Duration? Duration { get; } = duration;
    public Description? Description { get; } = description;
}
