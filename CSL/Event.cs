namespace CSL;

public readonly struct Event(Duration? duration = null, Clock? clock = null, Description? description = null)
{
    public Duration? Duration { get; } = duration;
    public Clock? Clock { get; } = clock;
    public Description? Description { get; } = description;
}
