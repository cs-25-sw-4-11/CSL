namespace CSL;

public record Event(
    Subject? Subject = null,
    Date? Date = null,
    Clock? Clock = null,
    Duration? Duration = null,
    Description? Description = null);
