namespace CSL;

public record Event(
    Subject? Subject = null,
    Date? Date = null,
    Clock? Clock = null,
    Duration? Duration = null,
    Description? Description = null)
{
    public static Event ConcatOperator(Event left, Event right)
    {
        if (left.Subject.HasValue && right.Subject.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Subject)}");
        }
        
        if (left.Date.HasValue && right.Date.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Date)}");
        }
        
        if (left.Clock.HasValue && right.Clock.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Clock)}");
        }
        
        if (left.Duration.HasValue && right.Duration.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Duration)}");
        }
        
        if (left.Description.HasValue && right.Description.HasValue)
        {
            throw new ArgumentException($"Overlapping {nameof(Description)}");
        }

        return new Event(
            Subject: left.Subject ?? right.Subject,
            Date: left.Date ?? right.Date,
            Clock: left.Clock ?? right.Clock,
            Duration: left.Duration ?? right.Duration,
            Description: left.Description ?? right.Description
        );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static explicit operator Event(Calendar c)
    {
        if (c.Events.Length != 1)
        {
            throw new ArgumentException();
        }

        return c.Events[0];
    }
}
