using System.Text;

namespace CSL;

using EventTypes;

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
    
    /// <summary>
    /// Tries to get the datetime for an event.
    /// If an event has a clock, then it gets added as well.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>Whether the datetime was able to be constructed.</returns>
    public bool TryGetDateTime(out DateTime dateTime)
    {
        if (Date is null)
        {
            dateTime = default;
            return false;
        }

        if (Clock.HasValue)
        {
            dateTime = new DateTime(Date.Value.Years,
                Date.Value.Months,
                Date.Value.Days,
                Clock.Value.Hours,
                Clock.Value.Minutes,
                0);
            return true;
        }

        dateTime = new DateTime(
            Date.Value.Years,
            Date.Value.Months,
            Date.Value.Days);
        return true;
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
        // checks if one of the events is only duration and puts it as the first operand
        if (left is { Duration: not null, Subject: null, Description: null, Clock: null, Date: null })
        {
            firstOperand = left;
            otherOperand = right;
        }
        else if (right is { Duration: not null, Subject: null, Description: null, Clock: null, Date: null })
        {
            firstOperand = right;
            otherOperand = left;
        }
        else
        {
            throw new ArgumentException($"Missing expression with only {nameof(Duration)}");
        }

        // checks if the other operand is a duration, date, dateclock or clock and calculates accordingly 
        if (otherOperand is { Duration: not null, DateClock: null })
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

            if (firstOperand.Duration.Value.Minutes % CSL.EventTypes.Duration.DayFactor == 0)
            {
                return new Event(
                    Date: otherOperand.Date + firstOperand.Duration
                );
            }

            var date = otherOperand.Date.Value;
            var dur = firstOperand.Duration.Value;
            var result1 = CSL.EventTypes.Date.Plus(date, dur);

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

        throw new ArgumentException(
            $"Missing expression with either {nameof(Duration)} or {nameof(Date)} and {nameof(Clock)} ");
    }

    public static Event SubOperator(Event left, Event right)
    {
        if (right.Duration is null)
        {
            throw new ArgumentException(
                $"Missing expression with {nameof(Duration)}");
        }
        if (left.Duration is not null && right.Date is not null || right.Clock is not null){
            throw new ArgumentException(
                $"Can not have both {nameof(Duration)} and ({nameof(Date)} or {nameof(Clock)})");
        }

        if (left.DateClock.HasValue)
        {
            if (left.DateClock.Value >= right.Duration)
            {
                return new Event(
                    Subject: left.Subject,
                    Description: left.Description,
                    dateClock: left.DateClock.Value - right.Duration.Value
                );
            }
        }

        if (left.Date.HasValue)
        {
            if (right.Duration.Value.Minutes % CSL.EventTypes.Duration.DayFactor == 0)
            {
                if (left.Date.Value >= right.Duration)
                {
                    return new Event(
                        Subject: left.Subject,
                        Description: left.Description,
                        Date: left.Date.Value - right.Duration.Value
                    );
                }
            }
            else
            {
                return new Event(
                        dateClock: CSL.EventTypes.Date.Minus(left.Date.Value, right.Duration.Value),
                        Subject: left.Subject,
                        Description: left.Description
                    );
            }
        }
        if (left.Clock.HasValue)
        {
            return new Event(
                Subject: left.Subject,
                Description: left.Description,
                Clock: left.Clock.Value - right.Duration.Value
            );
        }

        if (left.Duration is not null)
        {
            if (left.Duration >= right.Duration)
            {
                return new Event(
                    Subject: left.Subject,
                    Description: left.Description,
                    Duration: left.Duration - right.Duration
                );
            }
        }

        throw new ArgumentException(
            $"Missing expression with either {nameof(Duration)} or {nameof(Date)} and {nameof(Clock)} ");
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