using System.Text;

namespace CSL;

public record Event(
    Subject? Subject = null,
    Date? Date = null,
    Clock? Clock = null,
    Duration? Duration = null,
    Description? Description = null)
{
    public DateClock? DateClock
    {
        get
        {
            if (Date is null || Clock is null)
            {
                return null;
            }
            return new(Date.Value, Clock.Value);
        }
    }

    public Event(DateClock dateClock,
        Subject? Subject = null,
        Duration? Duration = null,
        Description? Description = null)
        : this(
            Subject: Subject,
            Date: dateClock.Date,
            Clock: dateClock.Clock,
            Duration: Duration,
            Description: Description)
    {
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('(');

        if (Subject.HasValue)
        {
            sb.Append($"subj:{Subject.Value}, ");
        }

        if (Date.HasValue)
        {
            sb.Append($"date:{Date.Value}, ");
        }

        if (Clock.HasValue)
        {
            sb.Append($"clock:{Clock.Value}, ");
        }

        if (Duration.HasValue)
        {
            sb.Append($"duration:{Duration.Value}, ");
        }

        if (Description.HasValue)
        {
            sb.Append($"description:{Description.Value}, ");
        }

        sb.Append(")");
        return sb.ToString();
    }

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

    public static Event AddOperator(Event left, Event right)
    {
        Event otherOperand;
        Event firstOperand;
        if (left.Duration.HasValue && !left.Subject.HasValue && !left.Description.HasValue && !left.Clock.HasValue && !left.Date.HasValue)
        {
            firstOperand = left;
            otherOperand = right;
        }
        else if (right.Duration.HasValue && !right.Subject.HasValue && !right.Description.HasValue && !right.Clock.HasValue && !right.Date.HasValue )
        {
            firstOperand = right;
            otherOperand = left;
        }
        else
        {
            throw new ArgumentException($"Missing expression with only {nameof(Duration)}");
        }
        
        if (otherOperand.Duration.HasValue && otherOperand.DateClock is null)
        {
            return new Event(
                Duration: firstOperand.Duration + otherOperand.Duration
            );
        }
        if (otherOperand.Duration is null && otherOperand.Date.HasValue)
        {
            
            if (otherOperand.Duration is null && otherOperand.DateClock.HasValue)
            {
                var dateclock = otherOperand.DateClock.Value;
                var duration = firstOperand.Duration;
                var result = dateclock + duration;
            
                return new Event(
                    (DateClock)(result)
                );
            }

            if (firstOperand.Duration.Value.Minutes % CSL.Duration.DayFactor == 0)
            {
                return new Event(
                    Date: otherOperand.Date + firstOperand.Duration
                );
            }
            
            var date = otherOperand.Date.Value;
            var dur = firstOperand.Duration.Value;
            var result1 = CSL.Date.Plus(date, dur);
            
            return new Event(
                result1
            );
        }
        if (otherOperand.Duration is null && otherOperand.Clock.HasValue)
        {
            return new Event(
                Clock: otherOperand.Clock + firstOperand.Duration
            );
        }
        
        
        throw new ArgumentException($"Missing expression with either {nameof(Duration)} or {nameof(Date)} and {nameof(Clock)} ");
        
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
